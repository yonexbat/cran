using cran.Data;
using cran.Model.Dto;
using cran.Model.Entities;
using cran.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Principal;
using System.Threading.Tasks;

namespace cran.Services
{
    public class QuestionService : CraniumService, IQuestionService
    {

        private readonly ICommentsService _commentsService;
        private readonly ITagService _tagService;
        private readonly ISecurityService _securityService;
       

        public QuestionService(ApplicationDbContext context, 
            IDbLogService dbLogService, 
            IPrincipal principal,
            ICommentsService commentsService,
            ITagService tagService,
            ISecurityService securityService) :
            base(context, dbLogService, securityService)
        {
            _tagService = tagService;
            _commentsService = commentsService;
            _securityService = securityService;
        }

        public async Task<int> InsertQuestionAsync(QuestionDto questionDto)
        {
            await _dbLogService.LogMessageAsync("Adding question");

            Container container = new Container();
            _context.Containers.Add(container);                        
            Question questionEntity = new Question();
            questionDto.QuestionType = questionDto.QuestionType == QuestionType.Unknown ?
                    QuestionType.MultipleChoice : questionDto.QuestionType;
            CopyData(questionDto, questionEntity);
            questionEntity.User = await GetCranUserAsync();
            questionEntity.Container = container;
            await _context.AddAsync(questionEntity);
            await SaveChangesAsync();
            questionDto.Id = questionEntity.Id;
            await UpdateQuestionAsync(questionDto);

            return questionEntity.Id;         
        }

        public async Task<QuestionDto> GetQuestionAsync(int id)
        {
            Question questionEntity = await _context.FindAsync<Question>(id);

            if(questionEntity == null)
            {
                throw new EntityNotFoundException(id, typeof(Question));
            }

            QuestionDto questionDto = new QuestionDto
            {
                Id = questionEntity.Id,
                Text = questionEntity.Text,
                Title = questionEntity.Title,
                QuestionType = questionEntity.QuestionType,
                Explanation = questionEntity.Explanation,
                Status = (int)questionEntity.Status,
                Language = questionEntity.Language.ToString(),
            };

            //Authorization
            questionDto.IsEditable = await HasWriteAccess(questionEntity.IdUser);

            //Vote-Statistics
            questionDto.Votes = await _commentsService.GetVoteAsync(id);

            //Options
            questionDto.Options = await _context.QuestionOptions
                .Where(x => x.IdQuestion == id)
                .OrderBy(x => x.Id)
                .Select(x => new QuestionOptionDto
                {
                    Id = x.Id,
                    IsTrue = x.IsTrue,
                    Text = x.Text,
                }).ToListAsync();

            //Tags
            questionDto.Tags = await _context.RelQuestionTags
                .Where(x => x.IdQuestion == id)               
                .Select(x => new TagDto
                {
                    Id = x.Tag.Id,
                    Name = x.Tag.Name,
                    Description = x.Tag.Description,
                    ShortDescDe = x.Tag.ShortDescDe,
                    ShortDescEn = x.Tag.ShortDescEn,
                    IdTagType = (int) x.Tag.TagType,
                }).ToListAsync();
          

            //Images
            questionDto.Images = await _context.RelQuestionImages
                .Where(x => x.IdQuestion == id)
                .Select(x => new ImageDto
                {
                    Id = x.Image.Id,
                    IdBinary = x.Image.Binary.Id,
                    Full = x.Image.Full,
                    Height = x.Image.Height,
                    Width = x.Image.Width,
                }).ToListAsync();

            return questionDto;
        }
             

        public async Task UpdateQuestionAsync(QuestionDto questionDto)
        {
            await CheckWriteAccessToQuestion(questionDto.Id);

            //set the parent id
            foreach (var optionDto in questionDto.Options)
            {
                optionDto.IdQuestion = questionDto.Id;
            }

            Question questionEntity = await _context.FindAsync<Question>(questionDto.Id);

            //Options
            IList<QuestionOption> questionOptionEntities = await _context.QuestionOptions.Where(x => x.IdQuestion == questionEntity.Id).ToListAsync();
            UpdateRelation(questionDto.Options, questionOptionEntities);

            //QuestionType
            questionEntity.QuestionType = questionDto.Options.Count(x => x.IsTrue) == 1 ?
                QuestionType.SingleChoice : QuestionType.MultipleChoice;

            //Tags
            IList<RelQuestionTag> relTagEntities = await _context.RelQuestionTags
                .Where(x => x.IdQuestion == questionEntity.Id).ToListAsync();
            relTagEntities = relTagEntities.GroupBy(x => x.IdTag).Select(x => x.First()).ToList();
            IDictionary<int, int> relIdByTagId = relTagEntities.ToDictionary(x => x.IdTag, x => x.Id);
            IList<RelQuestionTagDto> relQuestionTagDtos = new List<RelQuestionTagDto>();
            IList<TagDto> tagDtos = questionDto.Tags.Where(x => x.IdTagType == (int) TagType.Standard)
                .GroupBy(x => x.Id).Select(x => x.First()).ToList();

            foreach (TagDto tagDto in tagDtos)
            {
                RelQuestionTagDto relQuestionTagDto = new RelQuestionTagDto();
                relQuestionTagDto.IdTag = tagDto.Id;
                relQuestionTagDto.IdQuestion = questionDto.Id;
                if (relIdByTagId.ContainsKey(tagDto.Id))
                {
                    relQuestionTagDto.Id = relIdByTagId[tagDto.Id];
                }

                relQuestionTagDtos.Add(relQuestionTagDto);
            }

            UpdateRelation(relQuestionTagDtos, relTagEntities);

            //Image Relation
            IList<RelQuestionImage> relImages = await _context.RelQuestionImages.Where(x => x.IdQuestion == questionEntity.Id).ToListAsync();
            IDictionary<int, int> relIdByImageId = relImages.ToDictionary(x => x.IdImage, x => x.Id);
            IList<RelQuestionImageDto> relImagesDtos = new List<RelQuestionImageDto>();
            IEnumerable<int> binaryIds = questionDto.Images.Select(x => x.IdBinary);
            IList<Image> images = await _context.Images.Where(x => binaryIds.Contains(x.IdBinary)).ToListAsync();
            IDictionary<int, Image> imageByBinaryId = images.ToDictionary(x => x.IdBinary, x => x);
            foreach (ImageDto image in questionDto.Images)
            {
                RelQuestionImageDto relQuestionImageDto = new RelQuestionImageDto();
                relQuestionImageDto.IdQuestion = questionDto.Id;
                relQuestionImageDto.IdImage = imageByBinaryId[image.IdBinary].Id;
                if (relIdByImageId.ContainsKey(relQuestionImageDto.IdImage))
                {
                    relQuestionImageDto.Id = relIdByImageId[relQuestionImageDto.IdImage];
                }
                relImagesDtos.Add(relQuestionImageDto);
            }
            UpdateRelation(relImagesDtos, relImages);

            //Image Data           
            foreach (ImageDto imageDto in questionDto.Images)
            {
                Image image = imageByBinaryId[imageDto.IdBinary];
                CopyData(imageDto, image);
            }


            CopyData(questionDto, questionEntity);

            await SaveChangesAsync();            
        }

        public async Task<ImageDto> AddImageAsync(ImageDto imageDto)
        {
            Binary binary = await _context.FindAsync<Binary>(imageDto.IdBinary);

            Image image = new Image
            {
                Binary = binary,
                IdBinary = imageDto.IdBinary,
                Full = imageDto.Full,
                Height = imageDto.Height,
                Width = imageDto.Width,
            };
            _context.Images.Add(image);

            await SaveChangesAsync();

            imageDto.Id = image.Id;
            return imageDto;
        }


        public async Task DeleteQuestionAsync(int idQuestion)
        {

            await CheckWriteAccessToQuestion(idQuestion);

            Question questionEntity = await _context.FindAsync<Question>(idQuestion);

            //Options
            IList<QuestionOption> questionOptions = await _context.QuestionOptions.Where(x => x.Question.Id == questionEntity.Id).ToListAsync();
            foreach (QuestionOption questionOptionEntity in questionOptions)
            {
                _context.Remove(questionOptionEntity);

                //CourseInstanceQuestionOption
                IList<CourseInstanceQuestionOption> courseInstacesQuestionOption = await _context.CourseInstancesQuestionOption.Where(x => x.QuestionOption.Id == questionOptionEntity.Id).ToListAsync();
                foreach (CourseInstanceQuestionOption ciqo in courseInstacesQuestionOption)
                {
                    _context.Remove(ciqo);
                }
            }

            //Tags
            IList<RelQuestionTag> relTags = await _context.RelQuestionTags.Where(x => x.Question.Id == questionEntity.Id).ToListAsync();
            foreach (RelQuestionTag relTagEntity in relTags)
            {
                _context.Remove(relTagEntity);
            }

            //CourseInstance Question
            IList<CourseInstanceQuestion> courseInstaceQuestions = await _context.CourseInstancesQuestion.Where(x => x.Question.Id == questionEntity.Id).ToListAsync();
            foreach (CourseInstanceQuestion ciQ in courseInstaceQuestions)
            {
                _context.Remove(ciQ);
            }

            //Images
            IList<RelQuestionImage> relImages = await _context.RelQuestionImages.Where(x => x.Question.Id == questionEntity.Id).ToListAsync();
            foreach (RelQuestionImage relImage in relImages)
            {
                _context.Remove(relImage);
            }

            //Ratings
            IList<Rating> ratigns = await _context.Ratings.Where(x => x.Question.Id == questionEntity.Id).ToListAsync();
            foreach(Rating rating in ratigns)
            {
                _context.Remove(rating);
            }

            //Comments
            IList<Comment> comments = await _context.Comments.Where(x => x.Question.Id == questionEntity.Id).ToListAsync();
            foreach(Comment comment in comments)
            {
                _context.Remove(comment);
            }

            //Copy sources
            IList<Question> copies = await _context.Questions.Where(x => x.IdQuestionCopySource == idQuestion).ToListAsync();
            foreach(Question copy in copies)
            {
                copy.IdQuestionCopySource = null;
            }
            _context.Remove(questionEntity);
            await SaveChangesAsync();
           
        }

       
       

        public async Task<PagedResultDto<QuestionListEntryDto>> GetMyQuestionsAsync(int page)
        {
            string userId = _securityService.GetUserId();

            IQueryable<Question> query = _context.Questions.Where(q => q.User.UserId == userId)
                .OrderBy(x => x.Title)
                .ThenBy(x => x.Id);
            PagedResultDto<QuestionListEntryDto> result = await ToPagedResult(query, page, MaterializeQuestionList);
            

            return result;            
        }

        public async Task<PagedResultDto<QuestionListEntryDto>> SearchForQuestionsAsync(SearchQParametersDto parameters)
        {

            IQueryable<Question> queryBeforeSkipAndTake = _context.Questions
                .OrderBy(x => x.Title)
                .ThenBy(x => x.Id);

            if (!string.IsNullOrWhiteSpace(parameters.Title))
            {
                queryBeforeSkipAndTake = queryBeforeSkipAndTake.Where(x => x.Title.Contains(parameters.Title));
            }

            if (parameters.AndTags.Any())
            {
                IList<int> tagids = parameters.AndTags.Select(x => x.Id).ToList();
                foreach (int tagId in tagids)
                {
                    queryBeforeSkipAndTake = queryBeforeSkipAndTake.Where(x => x.RelTags.Any(rel => rel.Tag.Id == tagId));
                }
            }

            if (parameters.OrTags.Any())
            {
                IList<int> tagids = parameters.OrTags.Select(x => x.Id).ToList();
                queryBeforeSkipAndTake = queryBeforeSkipAndTake.Where(x => x.RelTags.Any(rel => tagids.Contains(rel.Tag.Id)));
            }

            if(parameters.Language.HasValue)
            {
                queryBeforeSkipAndTake = queryBeforeSkipAndTake.Where(x => x.Language == parameters.Language);
            }

            if(parameters.StatusCreated || parameters.StatusReleased || parameters.StatusObsolete)
            {
                queryBeforeSkipAndTake = queryBeforeSkipAndTake.Where(x => 
                    x.Status == QuestionStatus.Created && parameters.StatusCreated ||
                    x.Status == QuestionStatus.Released && parameters.StatusReleased ||
                    x.Status == QuestionStatus.Obsolete && parameters.StatusObsolete);
            }
            else
            {
                queryBeforeSkipAndTake = queryBeforeSkipAndTake.Where(x =>
                   x.Status == QuestionStatus.Created ||
                   x.Status == QuestionStatus.Released);
            }

            PagedResultDto<QuestionListEntryDto> result = await ToPagedResult(queryBeforeSkipAndTake, parameters.Page, MaterializeQuestionList);
            return result;
        }

        public async Task CheckWriteAccessToQuestion(int idQuestion)
        {
            //Security Check
            Question question = await _context.FindAsync<Question>(idQuestion);
            bool hasWriteAccess = await HasWriteAccess(question.IdUser);

            if (!hasWriteAccess)
            {
                throw new SecurityException("no access to this question");
            }
        }

        private async Task<IList<QuestionListEntryDto>> MaterializeQuestionList(IQueryable<Question> query)
        {
            IQueryable<int> questionIds = query.Select(q => q.Id);
            return await MaterializeQuestionListItems(questionIds);
        }

        private async Task<IList<QuestionListEntryDto>> MaterializeQuestionListItems(IQueryable<int> questionIds)
        {           
            IList<QuestionListEntryDto> result = await _context.Questions.Where(x => questionIds.Contains(x.Id))
              .Select(q => new QuestionListEntryDto { Title = q.Title, Id = q.Id, Status = (int)q.Status })
              .ToListAsync();

            var relTags = await _context.RelQuestionTags.Where(rel => questionIds.Contains(rel.Question.Id))
                .Select(rel => new {
                    TagId = rel.Tag.Id,
                    QuestionId = rel.Question.Id,
                    TagName = rel.Tag.Name,
                    TagType = rel.Tag.TagType,
                    TagShortDescDe = rel.Tag.ShortDescDe,
                    TagShortDescEn = rel.Tag.ShortDescEn,
                })
                .ToListAsync();
            

            foreach (var relTag in relTags)
            {
                var dto = result.Where(x => x.Id == relTag.QuestionId).Single();
                dto.Tags.Add(new TagDto
                {
                    Id = relTag.TagId,
                    Name = relTag.TagName,
                    IdTagType = (int) relTag.TagType,
                    ShortDescDe = relTag.TagShortDescDe,
                    ShortDescEn = relTag.TagShortDescEn,
                });
            }
           

            return result;
        }       
    }
}
