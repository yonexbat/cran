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

        public async Task<InsertActionDto> InsertQuestionAsync(QuestionDto questionVm)
        {
            await _dbLogService.LogMessageAsync("Adding question");

            Question questionEntity = new Question();
            questionEntity.User = await GetCranUserAsync();
            await CopyData(questionVm, questionEntity);              
            
            _context.Questions.Add(questionEntity);           

            await _context.SaveChangesCranAsync(_currentPrincipal);
            return new InsertActionDto
            {
                NewId = questionEntity.Id,
                Status = "Ok",
            };
        }

        public async Task<CoursesListDto> CoursesAsync()
        {
            await _dbLogService.LogMessageAsync("courses");
            CoursesListDto result = new CoursesListDto();
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

 

        public async Task UpdateQuestionAsync(QuestionDto questionVm)
        {
            await CheckAccessToQuestion(questionVm.Id);

            Question questionEntity = await _context.FindAsync<Question>(questionVm.Id);         
            foreach(QuestionOption optionEntity in _context.QuestionOptions.Where(x => x.IdQuestion == questionEntity.Id))
            {                
                _context.Remove(optionEntity);
            }

            foreach(RelQuestionTag relTagEntity in _context.RelQuestionTags.Where(x => x.IdQuestion == questionEntity.Id))
            {
                _context.Remove(relTagEntity);
            }
            
            await CopyData(questionVm, questionEntity);
            await _context.SaveChangesCranAsync(_currentPrincipal); 
        }

        private async Task CopyData(QuestionDto questionVm, Question questionEntity)
        {
            questionEntity.Title = questionVm.Title;
            questionEntity.Text = questionVm.Text;
            questionEntity.Explanation = questionVm.Explanation;
            
            AddOptions(questionVm, questionEntity);
            await AddTags(questionVm, questionEntity);
        }

        private void AddOptions(QuestionDto questionVm, Question questionEntity)
        {
            foreach (QuestionOptionDto option in questionVm.Options)
            {
                QuestionOption optionEntity = new QuestionOption();
                optionEntity.Question = questionEntity;
                optionEntity.IsTrue = option.IsTrue;
                optionEntity.Text = option.Text;

                questionEntity.Options.Add(optionEntity);
                _context.QuestionOptions.Add(optionEntity);
            }
        }
    
        private async Task AddTags(QuestionDto questionVm, Question questionEntity)
        {
            foreach(TagDto tagVm in questionVm.Tags)
            {
                int tagId = tagVm.Id;
                Tag tag = await _context.FindAsync<Tag>(tagId);

                RelQuestionTag relTag = new RelQuestionTag();
                relTag.Tag = tag;
                relTag.Question = questionEntity;
                questionEntity.RelTags.Add(relTag);
                _context.RelQuestionTags.Add(relTag);
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
            _context.CourseInstances.Add(courseInstanceEntity);

            await _context.SaveChangesCranAsync(_currentPrincipal);
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
                .Select(x => x.Question.Id);

            return questionIds;
        }

        private async Task<CourseInstanceDto> GetNextQuestion(CourseInstance courseInstanceEntity)
        {
            CourseInstanceDto result = new CourseInstanceDto();
            result.IdCourse = courseInstanceEntity.IdCourse;
            result.IdCourseInstance = courseInstanceEntity.Id;

            Course courseEntity = _context.Find<Course>(courseInstanceEntity.IdCourse);

            result.NumQuestionsAlreadyAsked = _context.CourseInstancesQuestion.Where(x => x.CourseInstance.Id == courseInstanceEntity.Id)
                .Count();

            result.NumQuestionsTotal = courseEntity.NumQuestionsToAsk;

            //Possible Quetions Query
            IQueryable<int> questionIds = PossibleQuestionsQuery(courseInstanceEntity);

            int count = await questionIds.CountAsync();
            if(count == 0)
            {
                result.IdCourseInstanceQuestion = 0;
                result.NumQuestionsTotal = result.NumQuestionsAlreadyAsked;
                result.Done = true;                
            }
            else
            {
                if(count <= result.NumQuestionsTotal - result.NumQuestionsAlreadyAsked)
                {
                    result.NumQuestionsTotal = result.NumQuestionsAlreadyAsked + count;
                }
                int quesitonNo = random.Next(0, count - 1);
                int questionId = await questionIds.Skip(quesitonNo).FirstAsync();
                Question questionEntity = _context.Find<Question>(questionId);

                //Course instance question
                CourseInstanceQuestion courseInstanceQuestionEntity = new CourseInstanceQuestion
                {
                    CourseInstance = courseInstanceEntity,
                    Question = questionEntity,
                };
                _context.CourseInstancesQuestion.Add(courseInstanceQuestionEntity);                

                //Course instance question options
                foreach (QuestionOption questionOptionEntity in _context.QuestionOptions.Where(option => option.Question.Id == questionEntity.Id))
                {
                    CourseInstanceQuestionOption courseInstanceQuestionOptionEntity = new CourseInstanceQuestionOption();

                    courseInstanceQuestionOptionEntity.QuestionOption = questionOptionEntity;
                    //questionOptionEntity.CourseInstancesQuestionOption.Add(courseInstanceQuestionOptionEntity);

                    courseInstanceQuestionOptionEntity.CourseInstanceQuestion = courseInstanceQuestionEntity;
                    courseInstanceQuestionEntity.CourseInstancesQuestionOption.Add(courseInstanceQuestionOptionEntity);

                    _context.CourseInstancesQuestionOption.Add(courseInstanceQuestionOptionEntity);
                }

                await _context.SaveChangesCranAsync(_currentPrincipal);
                result.IdCourseInstanceQuestion = courseInstanceQuestionEntity.Id;
            }
            
           
            return result;
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
            await _context.SaveChangesCranAsync(_currentPrincipal);
            return result;
        }

        public async Task<QuestionToAskDto> GetQuestionToAskAsync(int courseInstanceQuestionId)
        {
            QuestionToAskDto questionToAskVm = new QuestionToAskDto();
            CourseInstanceQuestion questionInstanceEntity = await _context.FindAsync<CourseInstanceQuestion>(courseInstanceQuestionId);
            CourseInstance courseInstanceEntity = await _context.FindAsync<CourseInstance>(questionInstanceEntity.IdCourseInstance);
            Question questionEntity = await _context.FindAsync<Question>(questionInstanceEntity.IdQuestion);
            Course courseEntity = await _context.FindAsync<Course>(courseInstanceEntity.IdCourse);

            questionToAskVm.IdCourseInstanceQuestion = courseInstanceQuestionId;        
            questionToAskVm.Text = questionEntity.Text;            
            questionToAskVm.NumQuestionsAsked = await _context.CourseInstancesQuestion.Where(x => x.CourseInstance.Id == courseInstanceEntity.Id).CountAsync();

            int possibleQuestions = await PossibleQuestionsQuery(courseInstanceEntity).CountAsync();
            
            questionToAskVm.NumQuestions = possibleQuestions  <= courseEntity.NumQuestionsToAsk - questionToAskVm.NumQuestionsAsked ? possibleQuestions + questionToAskVm.NumQuestionsAsked : courseEntity.NumQuestionsToAsk;
            

            foreach (var o in _context.CourseInstancesQuestionOption.Where( x => x.CourseInstanceQuestion.Id == courseInstanceQuestionId)
                .Include(x => x.QuestionOption))
            {

                questionToAskVm.Options.Add(new QuestionOptionToAskDto
                {
                    CourseInstanceQuestionOptionId = o.Id,
                    Text = o.QuestionOption.Text,
                });

            }

            return questionToAskVm;
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
            CourseInstanceDto result = await this.GetNextQuestion(courseInstanceEntity);           
            result.AnsweredCorrectly = courseInstanceQuestionEntity.Correct;
            return result;
        }

        private async Task SaveAnswers(QuestionAnswerDto answer)
        {

            CourseInstanceQuestion courseInstanceQuestionEntity = await _context.FindAsync<CourseInstanceQuestion>(answer.IdCourseInstanceQuestion);
            //Antworten abspeichern
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
                options[i].Checked = answer.Answers[i];
                options[i].Correct = options[i].QuestionOption.IsTrue == answer.Answers[i];
            }
            courseInstanceQuestionEntity.Correct = options.All(x => x.Correct);
            courseInstanceQuestionEntity.AnsweredAt = DateTime.Now;

            await _context.SaveChangesCranAsync(_currentPrincipal);
        }

        public async Task<IList<QuestionListEntryDto>> GetMyQuestionsAsync()
        {
            string userId = GetUserId(); 
                        

            var result =  await _context.Questions.Where(q => q.User.UserId == userId)
                .OrderBy(q => q.Title)
                .Select(q => new QuestionListEntryDto { Title = q.Title, Id = q.Id })
                .ToListAsync();

            IQueryable<int> questionIds = _context.Questions.Where(q => q.User.UserId == userId).Select(q => q.Id);

            var relTags = await _context.RelQuestionTags.Where(rel => questionIds.Contains(rel.Question.Id))
                .Select(rel => new {TagId = rel.Tag.Id, QuestionId = rel.Question.Id, TagName = rel.Tag.Name })
                .ToListAsync();

            foreach(var relTag in relTags)
            {
                var dto = result.Where(x => x.Id == relTag.QuestionId).Single();
                dto.Tags.Add(new TagDto {
                    Id = relTag.TagId,
                    Name = relTag.TagName,
                });
                
            }
            
            return result;
        }

        private async Task CheckAccessToQuestion(int idQuestion)
        {
            Question questionEntity = await _context.FindAsync<Question>(idQuestion);
            CranUser userEntityOfQuestion = await _context.FindAsync<CranUser>(questionEntity.IdUser);

            //Security Check
            if (!(userEntityOfQuestion.UserId == GetUserId() || _currentPrincipal.IsInRole(Roles.Admin)))
            {
                throw new SecurityException("no access to this question");
            }
        }

        public async Task DeleteQuestionAsync(int idQuestion)
        {

            await CheckAccessToQuestion(idQuestion);

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
            await _context.SaveChangesAsync();
        }

        public async Task<ResultDto> GetCourseResult(int idCourseInstance)
        {
            ResultDto result = new ResultDto()
            {
                IdCourseInstance = idCourseInstance,
            };

            result.Questions = await _context.CourseInstancesQuestion.Where(x => x.CourseInstance.Id == idCourseInstance)
                .Include(x => x.Question)
                .Select(x => new QuestionResultDto {
                        IdCourseInstanceQuestion = x.Id,
                        Title = x.Question.Title,
                    }).ToListAsync();

            return result;
        }
    }
}
