using cran.Data;
using cran.Model.Dto;
using cran.Model.Entities;
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

        public QuestionService(ApplicationDbContext context, 
            IDbLogService dbLogService, 
            IPrincipal principal,
            ICommentsService commentsService) :
            base(context, dbLogService, principal)
        {
            _context = context;
            _currentPrincipal = principal;
            _commentsService = commentsService;
        }



        public async Task<int> InsertQuestionAsync(QuestionDto questionDto)
        {
            await _dbLogService.LogMessageAsync("Adding question");

            Question questionEntity = new Question();
            CopyData(questionDto, questionEntity);
            questionEntity.User = await GetCranUserAsync();
            _context.Add(questionEntity);
            await SaveChangesAsync();
            questionDto.Id = questionEntity.Id;
            await UpdateQuestionAsync(questionDto);

            return questionEntity.Id;         
        }

        public async Task<QuestionDto> GetQuestionAsync(int id)
        {
            Question questionEntity = await _context.FindAsync<Question>(id);
            QuestionDto questionVm = new QuestionDto
            {
                Id = questionEntity.Id,
                Text = questionEntity.Text,
                Title = questionEntity.Title,
                Explanation = questionEntity.Explanation,
                Status = (int)questionEntity.Status,
                Language = questionEntity.Language.ToString(),
            };
            questionVm.IsEditable = await HasWriteAccess(questionEntity.IdUser);
            questionVm.Votes = await _commentsService.GetVoteAsync(id);

            questionVm.Options = await _context.QuestionOptions
                .Where(x => x.IdQuestion == id)
                .OrderBy(x => x.Id)
                .Select(x => new QuestionOptionDto
                {
                    Id = x.Id,
                    IsTrue = x.IsTrue,
                    Text = x.Text,
                }).ToListAsync();

            questionVm.Tags = await _context.RelQuestionTags
                .Where(x => x.IdQuestion == id)
                .Select(x => new TagDto
                {
                    Id = x.Tag.Id,
                    Name = x.Tag.Name,
                    Description = x.Tag.Description,
                }).ToListAsync();

            questionVm.Images = await _context.RelQuestionImages
                .Where(x => x.IdQuestion == id)
                .Select(x => new ImageDto
                {
                    Id = x.Image.Id,
                    IdBinary = x.Image.Binary.Id,
                    Full = x.Image.Full,
                    Height = x.Image.Height,
                    Width = x.Image.Width,
                }).ToListAsync();

            return questionVm;
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

            //Tags
            IList<RelQuestionTag> relTagEntities = await _context.RelQuestionTags
                .Where(x => x.IdQuestion == questionEntity.Id).ToListAsync();
            relTagEntities = relTagEntities.GroupBy(x => x.IdTag).Select(x => x.First()).ToList();
            IDictionary<int, int> relIdByTagId = relTagEntities.ToDictionary(x => x.IdTag, x => x.Id);
            IList<RelQuestionTagDto> relQuestionTagDtos = new List<RelQuestionTagDto>();
            IList<TagDto> tagDtos = questionDto.Tags.GroupBy(x => x.Id).Select(x => x.First()).ToList();

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
            IList<int> binaryIds = questionDto.Images.Select(x => x.IdBinary).ToList();
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


            await _context.SaveChangesCranAsync(_currentPrincipal);
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

            _context.Remove(questionEntity);
            await SaveChangesAsync();
        }

        public async Task<IList<QuestionListEntryDto>> GetMyQuestionsAsync()
        {
            string userId = GetUserId();
            IQueryable<Question> query = _context.Questions.Where(q => q.User.UserId == userId).OrderBy(x => x.Title);
            var result = await MaterializeQuestionList(query);
            return result;
        }

        public async Task<PagedResultDto<QuestionListEntryDto>> SearchForQuestionsAsync(SearchQParametersDto parameters)
        {


            IQueryable<Question> queryBeforeSkipAndTake = _context.Questions.OrderBy(x => x.Title);

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

            if(parameters.Status.HasValue)
            {
                queryBeforeSkipAndTake = queryBeforeSkipAndTake.Where(x => x.Status == parameters.Status);
            }

            PagedResultDto<QuestionListEntryDto> resultDto = new PagedResultDto<QuestionListEntryDto>();

            //Count und paging.
            int count = await queryBeforeSkipAndTake.CountAsync();
            int startindex = InitPagedResult(resultDto, count, parameters.Page);

            //Daten 
            IQueryable<Question> query = queryBeforeSkipAndTake.Skip(startindex).Take(PageSize);
            resultDto.Data = await MaterializeQuestionList(query);

            return resultDto;
        }

        private async Task CheckWriteAccessToQuestion(int idQuestion)
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
            IList<QuestionListEntryDto> result = await query
              .Select(q => new QuestionListEntryDto { Title = q.Title, Id = q.Id, Status = (int)q.Status })
              .ToListAsync();

            IQueryable<int> questionIds = query.Select(q => q.Id);

            var relTags = await _context.RelQuestionTags.Where(rel => questionIds.Contains(rel.Question.Id))
                .Select(rel => new { TagId = rel.Tag.Id, QuestionId = rel.Question.Id, TagName = rel.Tag.Name })
                .ToListAsync();

            foreach (var relTag in relTags)
            {
                var dto = result.Where(x => x.Id == relTag.QuestionId).Single();
                dto.Tags.Add(new TagDto
                {
                    Id = relTag.TagId,
                    Name = relTag.TagName,
                });

            }
            return result;
        }

        public async Task<int> CopyQuestionAsync(int id)
        {
            QuestionDto questionDto = await GetQuestionAsync(id);
            questionDto.Status = (int)QuestionStatus.Created;
            questionDto.Id = 0;
            return await InsertQuestionAsync(questionDto);
        }
    }
}
