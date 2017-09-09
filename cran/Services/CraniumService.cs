using cran.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cran.Model.Entities;
using System.Security.Principal;
using cran.Model.Dto;
using System.Security;
using cran.Model;

namespace cran.Services
{
    public class CraniumService : Service, ICraniumService
    {

        private IDbLogService _dbLogService;
        private static Random random = new Random(98789);


        public CraniumService(ApplicationDbContext context, IDbLogService dbLogService, IPrincipal principal) :
            base(context, principal)
        {
            _context = context;
            _dbLogService = dbLogService;
            _currentPrincipal = principal;
        }

        public async Task<InsertActionDto> InsertQuestionAsync(QuestionDto questionDto)
        {
            await _dbLogService.LogMessageAsync("Adding question");

            Question questionEntity = new Question();
            CopyData(questionDto, questionEntity);
            questionEntity.User = await GetCranUserAsync();
            _context.Add(questionEntity);
            await SaveChanges();
            questionDto.Id = questionEntity.Id;
            await UpdateQuestionAsync(questionDto);


            return new InsertActionDto
            {
                NewId = questionEntity.Id,
                Status = "Ok",
            };
        }

        public async Task<CoursesDto> GetCoursesAsync()
        {
            await _dbLogService.LogMessageAsync("courses");
            CoursesDto result = new CoursesDto();
            IList<Course> list = await this._context.Courses
                .Include(x => x.RelTags)
                .ThenInclude(x => x.Tag)                
                .ToListAsync();
            foreach (Course course in list)
            {
                CourseDto courseVm = new CourseDto
                {
                    Id = course.Id,
                    Title = course.Title,
                    Description = course.Description,
                };

                foreach(RelCourseTag relTag in course.RelTags)
                {
                    Tag tag = relTag.Tag;
                    TagDto tagVm = new TagDto
                    {
                        Description = tag.Description,
                        Name = tag.Name,
                    };
                    courseVm.Tags.Add(tagVm);
                }

                result.Courses.Add(courseVm);
            }

            return result;
        }

        public async Task<IList<TagDto>> FindTagsAsync(string searchTerm)
        {
            IList<Tag> tags = await _context.Tags.Where(x => x.Name.Contains(searchTerm)).ToListAsync();
            IList<TagDto> result = new List<TagDto>();
            
            foreach(Tag tag in tags)
            {
                TagDto tagVm = new TagDto
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    Description = tag.Description,
                };
                result.Add(tagVm);
            }
            return result;
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
                Status = (int) questionEntity.Status,
            };

            foreach(QuestionOption optionEntity in _context.QuestionOptions.Where(x => x.IdQuestion == id))
            {
                questionVm.Options.Add(new QuestionOptionDto
                {
                    Id = optionEntity.Id,
                    IsTrue = optionEntity.IsTrue,
                    Text = optionEntity.Text,
                });
            }

            foreach(RelQuestionTag relTag in _context.RelQuestionTags.Where(x => x.IdQuestion == id).Include(x => x.Tag))
            {
                questionVm.Tags.Add(new TagDto
                {
                    Id = relTag.Tag.Id,
                    Name = relTag.Tag.Name,
                    Description = relTag.Tag.Description,
                });
            }

            return questionVm;
        }

 

        public async Task UpdateQuestionAsync(QuestionDto questionDto)
        {
            await CheckWriteAccessToQuestion(questionDto.Id);

            //set the parent id
            foreach(var optionDto in questionDto.Options)
            {
                optionDto.IdQuestion = questionDto.Id;
            }

            Question questionEntity = await _context.FindAsync<Question>(questionDto.Id);

            //Options
            IList<QuestionOption> questionOptionEntities = await _context.QuestionOptions.Where(x => x.IdQuestion == questionEntity.Id).ToListAsync();
            UpdateRelation(questionDto.Options, questionOptionEntities);

            //Tags
            IList<RelQuestionTag> relTagEntities = await _context.RelQuestionTags.Where(x => x.IdQuestion == questionEntity.Id).ToListAsync();
            IList<RelQuestionTagDto> relQuestionTagDtos = new List<RelQuestionTagDto>();
            foreach(TagDto tagDto in questionDto.Tags)
            {
                RelQuestionTagDto relQuestionTagDto = new RelQuestionTagDto();
                relQuestionTagDto.IdTag = tagDto.Id;
                relQuestionTagDto.IdQuestion = questionDto.Id;
                relQuestionTagDto.Id = relTagEntities.Where(x => x.IdTag == tagDto.Id).Select(x => x.Id).SingleOrDefault();
                relQuestionTagDtos.Add(relQuestionTagDto);
            }
           
            UpdateRelation(relQuestionTagDtos, relTagEntities);
            

            CopyData(questionDto, questionEntity);
            await _context.SaveChangesCranAsync(_currentPrincipal); 
        }

        private void UpdateRelation<Tdto, Tentity>(IList<Tdto> dtos, IList<Tentity> entitties) 
            where Tdto: IDto 
            where Tentity : CranEntity, IIdentifiable, new()
        {
            IEnumerable<int> idsEntities = entitties.Select(x => x.Id);
            IEnumerable<int> idsDtos = dtos.Select(x => x.Id);
            IEnumerable<IIdentifiable> entitiesToDelete = entitties.Where(x => idsDtos.All(id => id != x.Id)).Cast<IIdentifiable>();
            IEnumerable<IIdentifiable> entitiesToUpdate = entitties.Where(x => idsDtos.Any(id => id == x.Id)).Cast<IIdentifiable>();
            IEnumerable<IIdentifiable> dtosToAdd = dtos.Where(x => x.Id <= 0).Cast<IIdentifiable>();
            
            //Delete
            foreach(IIdentifiable entity in entitiesToDelete)
            {
                _context.Remove(entity);
            }

            //Update
            foreach(CranEntity entity in entitiesToUpdate)
            {
                IDto dto = dtos.Single(x => x.Id == entity.Id);
                CopyData(dto, entity);
            }
            
            //Add
            foreach(IDto dto in dtosToAdd)
            {
                Tentity entity = new Tentity();
                CopyData(dto, entity);
                _context.Set<Tentity>().Add(entity);
            }
        }

        private void CopyData(IDto dto, CranEntity entity)
        {
            if(dto is QuestionOptionDto && entity is QuestionOption)
            {
                QuestionOptionDto dtoSource = (QuestionOptionDto)dto;
                QuestionOption entityDestination = (QuestionOption) entity;
                entityDestination.IsTrue = dtoSource.IsTrue;
                entityDestination.Text = dtoSource.Text ?? string.Empty;
                entityDestination.IdQuestion = dtoSource.IdQuestion;
            }
            else if (dto is QuestionDto && entity is Question)
            {
                QuestionDto dtoSource = (QuestionDto )dto;
                Question entityDestination = (Question)entity;
                entityDestination.Title = dtoSource.Title;
                entityDestination.Text = dtoSource.Text ?? string.Empty;
                entityDestination.Explanation = dtoSource.Explanation;
                entityDestination.Status = (QuestionStatus) dtoSource.Status;
            }
            else if(dto is RelQuestionTagDto && entity is RelQuestionTag)
            {
                RelQuestionTagDto dtoSource = (RelQuestionTagDto)dto;
                RelQuestionTag entityDestination = (RelQuestionTag)entity;
                entityDestination.IdQuestion = dtoSource.IdQuestion;
                entityDestination.IdTag = dtoSource.IdTag;
            }
            else
            {
                throw new NotImplementedException();
            }                  
        }

        public async Task<CourseInstanceDto> StartCourseAsync(int courseId)
        {
            Course courseEntity = await _context.FindAsync<Course>(courseId);
            CranUser cranUserEntity = await GetCranUserAsync();

            CourseInstance courseInstanceEntity = new CourseInstance
            {
                User = cranUserEntity,
                Course = courseEntity,
                IdCourse = courseId,
            };
            _context.Add(courseInstanceEntity);

            await SaveChanges();
            CourseInstanceDto result = await GetNextQuestion(courseInstanceEntity);                        

            return result;


        }

        private IQueryable<int> PossibleQuestionsQuery(CourseInstance courseInstanceEntity)
        {
            //Get Tags of course
            IQueryable<int> tagIds = _context.RelCourseTags.Where(x => x.Course.Id == courseInstanceEntity.IdCourse)
                .Select(x => x.Tag.Id);

            //Questions already asked
            IQueryable<int> questionIdsAlreadyAsked = _context.CourseInstancesQuestion
                .Where(x => x.CourseInstance.Id == courseInstanceEntity.Id)
                .Select(x => x.Question.Id);

            //Possible Quetions Query
            IQueryable<int> questionIds = _context.RelQuestionTags
                .Where(x => tagIds.Contains(x.Tag.Id))
                .Where(x => !questionIdsAlreadyAsked.Contains(x.Question.Id))
                .Where(x => x.Question.Status == QuestionStatus.Released)
                .Select(x => x.Question.Id);

            return questionIds;
        }

        private async Task<CourseInstanceDto> GetNextQuestion(CourseInstance courseInstanceEntity)
        {
            CourseInstanceDto result = new CourseInstanceDto();
            result.IdCourse = courseInstanceEntity.IdCourse;
            result.IdCourseInstance = courseInstanceEntity.Id;

            Course courseEntity = await _context.FindAsync<Course>(courseInstanceEntity.IdCourse);

            result.NumQuestionsAlreadyAsked = await _context.CourseInstancesQuestion.Where(x => x.CourseInstance.Id == courseInstanceEntity.Id)
                .CountAsync();

            result.NumQuestionsTotal = courseEntity.NumQuestionsToAsk;

            //Possible Quetions Query
            IQueryable<int> questionIds = PossibleQuestionsQuery(courseInstanceEntity);

            int count = await questionIds.CountAsync();
            if(count == 0 || result.NumQuestionsAlreadyAsked >= courseEntity.NumQuestionsToAsk)
            {
                result.IdCourseInstanceQuestion = 0;
                result.NumQuestionsTotal = result.NumQuestionsAlreadyAsked;
                result.Done = true;
                await EndCourseAsync(courseInstanceEntity.Id);
            }
            else
            {
                if(count <= result.NumQuestionsTotal - result.NumQuestionsAlreadyAsked)
                {
                    result.NumQuestionsTotal = result.NumQuestionsAlreadyAsked + count;
                }                
                int quesitonNo = random.Next(0, count - 1);
                int questionId = await questionIds.Skip(quesitonNo).FirstAsync();
                Question questionEntity = await _context.FindAsync<Question>(questionId);

                //Course instance question
                CourseInstanceQuestion courseInstanceQuestionEntity = new CourseInstanceQuestion
                {
                    CourseInstance = courseInstanceEntity,
                    Question = questionEntity,
                };
                _context.Add(courseInstanceQuestionEntity);

                //Course instance question options
                IList<QuestionOption> options = await _context.QuestionOptions.Where(option => option.Question.Id == questionEntity.Id).ToListAsync();
                foreach (QuestionOption questionOptionEntity in options)
                {
                    CourseInstanceQuestionOption courseInstanceQuestionOptionEntity = new CourseInstanceQuestionOption();

                    courseInstanceQuestionOptionEntity.QuestionOption = questionOptionEntity;

                    courseInstanceQuestionOptionEntity.CourseInstanceQuestion = courseInstanceQuestionEntity;
                    courseInstanceQuestionEntity.CourseInstancesQuestionOption.Add(courseInstanceQuestionOptionEntity);

                    _context.Add(courseInstanceQuestionOptionEntity);
                }

                await SaveChanges();
                result.IdCourseInstanceQuestion = courseInstanceQuestionEntity.Id;
            }
            
           
            return result;
        }

        private async Task EndCourseAsync(int courseInstanceId)
        {
            CourseInstance courseInstance = await _context.FindAsync<CourseInstance>(courseInstanceId);
            courseInstance.EndedAt = DateTime.Now;
            await SaveChanges();
        }

        private async Task<CranUser> GetCranUserAsync()
        {
            string userId = GetUserId();
            CranUser cranUserEntity = await _context.CranUsers.Where(x => x.UserId == userId).SingleOrDefaultAsync();           
            if(cranUserEntity == null)
            {
                cranUserEntity = new CranUser
                {
                    UserId = userId,
                };
                _context.CranUsers.Add(cranUserEntity);
            }
            return cranUserEntity;
        }

        public async Task<CourseInstanceDto> NextQuestion(int courseInstanceId)
        {
            CourseInstance courseInstanceEntity = _context.Find<CourseInstance>(courseInstanceId);          
            CourseInstanceDto result = await GetNextQuestion(courseInstanceEntity);          
            await SaveChanges();
            return result;
        }

        public async Task<QuestionToAskDto> GetQuestionToAskAsync(int courseInstanceQuestionId)
        {
            QuestionToAskDto questionToAskDto = new QuestionToAskDto();
            CourseInstanceQuestion questionInstanceEntity = await _context.FindAsync<CourseInstanceQuestion>(courseInstanceQuestionId);
            CourseInstance courseInstanceEntity = await _context.FindAsync<CourseInstance>(questionInstanceEntity.IdCourseInstance);
            Question questionEntity = await _context.FindAsync<Question>(questionInstanceEntity.IdQuestion);
            Course courseEntity = await _context.FindAsync<Course>(courseInstanceEntity.IdCourse);

            questionToAskDto.IdCourseInstanceQuestion = courseInstanceQuestionId;        
            questionToAskDto.Text = questionEntity.Text;            
            questionToAskDto.NumQuestionsAsked = await _context.CourseInstancesQuestion.Where(x => x.CourseInstance.Id == courseInstanceEntity.Id).CountAsync();
            questionToAskDto.CourseEnded = courseInstanceEntity.EndedAt.HasValue;
            questionToAskDto.IdCourseInstance = courseInstanceEntity.Id;

            int possibleQuestions = await PossibleQuestionsQuery(courseInstanceEntity).CountAsync();
            
            questionToAskDto.NumQuestions = possibleQuestions  <= courseEntity.NumQuestionsToAsk - questionToAskDto.NumQuestionsAsked ? possibleQuestions + questionToAskDto.NumQuestionsAsked : courseEntity.NumQuestionsToAsk;

            IList<CourseInstanceQuestionOption> optionInstanceEntities = await _context.CourseInstancesQuestionOption
                            .Where(x => x.CourseInstanceQuestion.Id == courseInstanceQuestionId)
                            .OrderBy(x => x.CourseInstanceQuestion.Id)
                            .Include(x => x.QuestionOption).ToListAsync();

            foreach (CourseInstanceQuestionOption optionInstanceEntity in optionInstanceEntities)
            {

                questionToAskDto.Options.Add(new QuestionOptionToAskDto
                {
                    CourseInstanceQuestionOptionId = optionInstanceEntity.Id,
                    Text = optionInstanceEntity.QuestionOption.Text,
                    IsChecked = optionInstanceEntity.Checked,
                });

            }

            return questionToAskDto;
        }

        public async Task<QuestionDto> AnswerQuestionAndGetSolutionAsync(QuestionAnswerDto answer)
        {
            await SaveAnswers(answer);

            int questionId = await _context.CourseInstancesQuestion.Where(x => x.Id == answer.IdCourseInstanceQuestion)
                .Select(x => x.Question.Id).SingleAsync();

            CourseInstanceQuestion courseInstanceQuestion = await _context.FindAsync<CourseInstanceQuestion>(answer.IdCourseInstanceQuestion);
            await _context.SaveChangesCranAsync(_currentPrincipal);
            
            return await GetQuestionAsync(questionId);
        }

        public async Task<CourseInstanceDto> AnswerQuestionAndGetNextQuestionAsync(QuestionAnswerDto answer)
        {
            await SaveAnswers(answer);

            //Nächste Frage vorbereiten.
            CourseInstanceQuestion courseInstanceQuestionEntity = await _context.FindAsync<CourseInstanceQuestion>(answer.IdCourseInstanceQuestion);
            CourseInstance courseInstanceEntity = await _context.FindAsync<CourseInstance>(courseInstanceQuestionEntity.IdCourseInstance);
            CourseInstanceDto result = await GetNextQuestion(courseInstanceEntity);           
            result.AnsweredCorrectly = courseInstanceQuestionEntity.Correct;
            return result;
        }

        private async Task SaveAnswers(QuestionAnswerDto answer)
        {

            CourseInstanceQuestion courseInstanceQuestionEntity = await _context.FindAsync<CourseInstanceQuestion>(answer.IdCourseInstanceQuestion);

            IList<CourseInstanceQuestionOption> options = await _context.CourseInstancesQuestionOption
                .Where(x => x.CourseInstanceQuestion.Id == courseInstanceQuestionEntity.Id)
                .OrderBy(x => x.Id)
                .Include(x => x.QuestionOption).ToListAsync();            

            if (options.Count != answer.Answers.Count)
            {
                throw new InvalidOperationException("something wrong");
            }
            for (int i = 0; i < options.Count; i++)
            {
                CourseInstanceQuestionOption optionInstanceEntity = options[i];
                bool thisAnswer = answer.Answers[i];
                bool optioncorrect = optionInstanceEntity.QuestionOption.IsTrue == thisAnswer;

                optionInstanceEntity.Checked = thisAnswer;
                optionInstanceEntity.Correct = optioncorrect;                               
            }
            courseInstanceQuestionEntity.Correct = options.All(x => x.Correct);
            courseInstanceQuestionEntity.AnsweredAt = DateTime.Now;

            await SaveChanges();
        }

        public async Task<IList<QuestionListEntryDto>> GetMyQuestionsAsync()
        {
            string userId = GetUserId();
            IQueryable<Question> query = _context.Questions.Where(q => q.User.UserId == userId).OrderBy(x => x.Title);
            var result =  await MaterializeQuestionList(query);          
            return result;
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

        private async Task CheckWriteAccessToQuestion(int idQuestion)
        {
            Question questionEntity = await _context.FindAsync<Question>(idQuestion);
            CranUser userEntityOfQuestion = await _context.FindAsync<CranUser>(questionEntity.IdUser);

            //Security Check
            if (!(userEntityOfQuestion.UserId == GetUserId() || _currentPrincipal.IsInRole(Roles.Admin)))
            {
                throw new SecurityException("no access to this question");
            }
        }

        private async Task CheckAccessToCourseInstance(int idCourseInstance)
        {
            CourseInstance instance = await _context.FindAsync<CourseInstance>(idCourseInstance);
            CranUser userEntityOfQuestion = await _context.FindAsync<CranUser>(instance.IdUser);

            //Security Check
            if (!(userEntityOfQuestion.UserId == GetUserId() || _currentPrincipal.IsInRole(Roles.Admin)))
            {
                throw new SecurityException("no access to this question");
            }
        }
      

        public async Task DeleteQuestionAsync(int idQuestion)
        {

            await CheckWriteAccessToQuestion(idQuestion);

            Question questionEntity =  await _context.FindAsync<Question>(idQuestion);
                        


            //Options
            IList<QuestionOption> questionOptions = await _context.QuestionOptions.Where(x => x.Question.Id == questionEntity.Id).ToListAsync();
            foreach (QuestionOption questionOptionEntity in questionOptions)
            {                
                _context.Remove(questionOptionEntity);

                //CourseInstanceQuestionOption
                IList<CourseInstanceQuestionOption> courseInstacesQuestionOption = await _context.CourseInstancesQuestionOption.Where(x => x.QuestionOption.Id == questionOptionEntity.Id).ToListAsync();
                foreach(CourseInstanceQuestionOption ciqo in courseInstacesQuestionOption)
                {
                    _context.Remove(ciqo);
                }
            }

            //Tags
            IList<RelQuestionTag> relTags = await _context.RelQuestionTags.Where(x => x.Question.Id == questionEntity.Id).ToListAsync();
            foreach(RelQuestionTag relTagEntity in relTags)
            {
                _context.Remove(relTagEntity);
            }

            //Instances
            IList<CourseInstanceQuestion> courseInstaceQuestions = await _context.CourseInstancesQuestion.Where(x => x.Question.Id == questionEntity.Id).ToListAsync();
            foreach(CourseInstanceQuestion ciQ  in courseInstaceQuestions)
            {
                _context.Remove(ciQ);
            }

            _context.Remove(questionEntity);
            await SaveChanges();
        }

        public async Task<ResultDto> GetCourseResultAsync(int idCourseInstance)
        {
            CourseInstance courseInstance = await _context.FindAsync<CourseInstance>(idCourseInstance);
            Course course = await _context.FindAsync<Course>(courseInstance.IdCourse);
            ResultDto result = new ResultDto()
            {
                IdCourseInstance = idCourseInstance,
                CourseTitle = course.Title,
            };

            result.Questions = await _context.CourseInstancesQuestion.Where(x => x.CourseInstance.Id == idCourseInstance)                
                .Select(x => new QuestionResultDto {
                        IdCourseInstanceQuestion = x.Id,
                        IdQuestion = x.Question.Id,
                        Title = x.Question.Title,
                        Correct = x.Correct,
                    }).ToListAsync();

            //Tags holen
            var tags = await _context.RelQuestionTags.Where(x => x.Question.CourseInstancesQuestion.Any(y => y.CourseInstance.Id == idCourseInstance))
                .Select(x => new
                {
                    IdQuestion = x.Question.Id,
                    IdTag = x.Tag.Id,
                    x.Tag.Name,
                    x.Tag.Description,
                }).ToListAsync();
            var tagLookups = tags.ToLookup(x => x.IdQuestion, x => x);

            foreach (QuestionResultDto questionDto in result.Questions)
            {
                if (tagLookups.Contains(questionDto.IdQuestion))
                {
                    var tagLookup = tagLookups[questionDto.IdQuestion];
                    foreach(var tag in tagLookup)
                    {
                        questionDto.Tags.Add(new TagDto
                        {
                            Id = tag.IdTag,
                            Description = tag.Description,
                            Name = tag.Name,
                        });
                    }
                }
            }

            return result;
        }

        public async Task<IList<CourseInstanceListEntryDto>> GetMyCourseInstancesAsync()
        {            
            string userid =  GetUserId();
            IQueryable<CourseInstanceListEntryDto> query = _context.CourseInstances.Where(x => x.User.UserId == userid)
                .Select(x => new CourseInstanceListEntryDto()
                {
                    IdCourseInstance = x.Id,
                    Title = x.Course.Title,
                    NumQuestionsCorrect = x.CourseInstancesQuestion.Count(y => y.Correct),
                    NumQuestionsTotal =  x.CourseInstancesQuestion.Count(),
                    InsertDate = x.InsertDate,
                })
                .OrderByDescending(x => x.InsertDate);

            IList<CourseInstanceListEntryDto> result = await query.ToListAsync();
            return result;
        }

        public async Task DeleteCourseInstanceAsync(int idCourseInstance)
        {
            await CheckAccessToCourseInstance(idCourseInstance);

            CourseInstance instance = await _context.FindAsync<CourseInstance>(idCourseInstance);            


            IList<CourseInstanceQuestionOption> courseInstanceQuestionOptions =
                    await _context.CourseInstancesQuestionOption
                    .Where(x => x.CourseInstanceQuestion.CourseInstance.Id == instance.Id)
                    .ToListAsync();

            foreach(CourseInstanceQuestionOption courseInstanceQuestionOption in courseInstanceQuestionOptions)
            {
                _context.CourseInstancesQuestionOption.Remove(courseInstanceQuestionOption);
            }

            IList<CourseInstanceQuestion> coureInstanceQuestions = await _context.CourseInstancesQuestion
                .Where(x => x.CourseInstance.Id == instance.Id)
                .ToListAsync();

            foreach(CourseInstanceQuestion courseInstanceQuestion in coureInstanceQuestions)
            {
                _context.Remove(courseInstanceQuestion);
            }

            _context.Remove(instance);

            await SaveChanges();

        }

        public async Task<PagedResultDto<QuestionListEntryDto>> SearchForQuestionsAsync(SearchQParametersDto parameters)
        {
            int pageSize = 5;
            int startindex = parameters.Page * pageSize;
            IQueryable<Question> queryBeforeSkipAndTake = _context.Questions.OrderBy(x => x.Title);
            
            if(!string.IsNullOrWhiteSpace(parameters.Title))
            {
                queryBeforeSkipAndTake = queryBeforeSkipAndTake.Where(x => x.Title.Contains(parameters.Title));
            }

            if(parameters.AndTags.Any())
            {
                IList<int> tagids = parameters.AndTags.Select(x => x.Id).ToList();
                foreach (int tagId in tagids)
                {
                    queryBeforeSkipAndTake = queryBeforeSkipAndTake.Where(x => x.RelTags.Any(rel => rel.Tag.Id == tagId));
                }
            }

            if(parameters.OrTags.Any())
            {
                IList<int> tagids = parameters.OrTags.Select(x => x.Id).ToList();
                queryBeforeSkipAndTake = queryBeforeSkipAndTake.Where(x => x.RelTags.Any(rel => tagids.Contains(rel.Tag.Id)));
            }

            IQueryable<Question> query = queryBeforeSkipAndTake.Skip(startindex).Take(pageSize);
            PagedResultDto<QuestionListEntryDto> resultDto = new PagedResultDto<QuestionListEntryDto>();
            resultDto.Pagesize = pageSize;
            resultDto.Data = await MaterializeQuestionList(query);
            int count = await queryBeforeSkipAndTake.CountAsync();
            resultDto.Numpages = count / pageSize + 1;
            resultDto.CurrentPage = parameters.Page;
            return resultDto;
        }

    }
}
