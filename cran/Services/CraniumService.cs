using cran.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cran.Model.Entities;
using cran.Model.ViewModel;
using System.Security.Principal;

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

        public async Task<InsertActionViewModel> AddQuestionAsync(QuestionViewModel questionVm)
        {
            await _dbLogService.LogMessageAsync("Adding question");
            Question questionEntity = new Question();
            await CopyData(questionVm, questionEntity);              
            
            _context.Questions.Add(questionEntity);           

            await _context.SaveChangesCranAsync(_currentPrincipal);

            return new InsertActionViewModel
            {
                NewId = questionEntity.Id,
                Status = "Ok",
            };
        }

        public async Task<CoursesListViewModel> CoursesAsync()
        {
            await _dbLogService.LogMessageAsync("courses");
            CoursesListViewModel result = new CoursesListViewModel();
            IList<Course> list = await this._context.Courses
                .Include(x => x.RelTags)
                .ThenInclude(x => x.Tag)                
                .ToListAsync();
            foreach (Course course in list)
            {
                CourseViewModel courseVm = new CourseViewModel
                {
                    Id = course.Id,
                    Title = course.Title,
                    Description = course.Description,
                };

                foreach(RelCourseTag relTag in course.RelTags)
                {
                    Tag tag = relTag.Tag;
                    TagViewModel tagVm = new TagViewModel
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

        public async Task<IList<TagViewModel>> FindTagsAsync(string searchTerm)
        {
            IList<Tag> tags = await _context.Tags.Where(x => x.Name.Contains(searchTerm)).ToListAsync();
            IList<TagViewModel> result = new List<TagViewModel>();
            
            foreach(Tag tag in tags)
            {
                TagViewModel tagVm = new TagViewModel
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    Description = tag.Description,
                };
                result.Add(tagVm);
            }
            return result;
        }

        public async Task<QuestionViewModel> GetQuestionAsync(int id)
        {
            Question questionEntity = await _context.FindAsync<Question>(id);
            QuestionViewModel questionVm = new QuestionViewModel
            {
                Id = questionEntity.Id,
                Text = questionEntity.Text,
                Title = questionEntity.Title,       
                Explanation = questionEntity.Explanation,
            };

            foreach(QuestionOption optionEntity in _context.QuestionOptions.Where(x => x.IdQuestion == id))
            {
                questionVm.Options.Add(new QuestionOptionViewModel
                {
                    Id = optionEntity.Id,
                    IsTrue = optionEntity.IsTrue,
                    Text = optionEntity.Text,
                });
            }

            foreach(RelQuestionTag relTag in _context.RelQuestionTags.Where(x => x.IdQuestion == id).Include(x => x.Tag))
            {
                questionVm.Tags.Add(new TagViewModel
                {
                    Id = relTag.Tag.Id,
                    Name = relTag.Tag.Name,
                    Description = relTag.Tag.Description,
                });
            }

            return questionVm;
        }

 

        public async Task SaveQuestionAsync(QuestionViewModel questionVm)
        {
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

        private async Task CopyData(QuestionViewModel questionVm, Question questionEntity)
        {
            questionEntity.Title = questionVm.Title;
            questionEntity.Text = questionVm.Text;
            questionEntity.Explanation = questionVm.Explanation;
            
            AddOptions(questionVm, questionEntity);
            await AddTags(questionVm, questionEntity);
        }

        private void AddOptions(QuestionViewModel questionVm, Question questionEntity)
        {
            foreach (QuestionOptionViewModel option in questionVm.Options)
            {
                QuestionOption optionEntity = new QuestionOption();
                optionEntity.Question = questionEntity;
                optionEntity.IsTrue = option.IsTrue;
                optionEntity.Text = option.Text;

                questionEntity.Options.Add(optionEntity);
                _context.QuestionOptions.Add(optionEntity);
            }
        }
    
        private async Task AddTags(QuestionViewModel questionVm, Question questionEntity)
        {
            foreach(TagViewModel tagVm in questionVm.Tags)
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

        public async Task<CourseInstanceViewModel> StartCourseAsync(int courseId)
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
            CourseInstanceViewModel result = await GetNextQuestion(courseInstanceEntity);                        

            return result;


        }

        private async Task<CourseInstanceViewModel> GetNextQuestion(CourseInstance courseInstanceEntity)
        {
            CourseInstanceViewModel result = new CourseInstanceViewModel();
            result.IdCourse = courseInstanceEntity.IdCourse;
            result.IdCourseInstance = courseInstanceEntity.Id;

            Course courseEntity = _context.Find<Course>(courseInstanceEntity.IdCourse);

            result.NumQuestionsAlreadyAsked = _context.CourseInstancesQuestion.Where(x => x.CourseInstance.Id == courseInstanceEntity.Id)
                .Count();

            result.NumQuestionsTotal = courseEntity.NumQuestionsToAsk;

            //Get Tags of course
            IQueryable<int> tagIds =  _context.RelCourseTags.Where(x => x.Course.Id == courseInstanceEntity.IdCourse)
                .Select(x => x.Tag.Id);

            //Questions already asked
            IQueryable<int> questionIdsAlreadyAsked = _context.CourseInstancesQuestion
                .Where(x => x.CourseInstance.Id == courseInstanceEntity.Id)
                .Select(x => x.Question.Id);

            //Possible Quetions Query
            IQueryable<int> questionIds =_context.RelQuestionTags
                .Where(x => tagIds.Contains(x.Tag.Id))
                .Where(x => !questionIdsAlreadyAsked.Contains(x.Question.Id))
                .Select(x => x.Question.Id);            

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

        public async Task<CourseInstanceViewModel> NextQuestion(int courseInstanceId)
        {
            CourseInstance courseInstanceEntity = _context.Find<CourseInstance>(courseInstanceId);          
            CourseInstanceViewModel result = await GetNextQuestion(courseInstanceEntity);          
            await _context.SaveChangesCranAsync(_currentPrincipal);
            return result;
        }

        public async Task<QuestionToAskViewModel> GetQuestionToAskAsync(int courseInstanceQuestionId)
        {
            QuestionToAskViewModel questionToAskVm = new QuestionToAskViewModel();
            CourseInstanceQuestion questionInstanceEntity = await _context.FindAsync<CourseInstanceQuestion>(courseInstanceQuestionId);
            
            Question questionEntity = await _context.FindAsync<Question>(questionInstanceEntity.IdQuestion);

            questionToAskVm.IdCourseInstanceQuestion = courseInstanceQuestionId;

            questionToAskVm.Text = questionEntity.Text;

            foreach(var o in _context.CourseInstancesQuestionOption.Where( x => x.CourseInstanceQuestion.Id == courseInstanceQuestionId)
                .Include(x => x.QuestionOption))
            {

                questionToAskVm.Options.Add(new QuestionOptionToAskViewModel
                {
                    CourseInstanceQuestionOptionId = o.Id,
                    Text = o.QuestionOption.Text,
                });

            }

            return questionToAskVm;
        }

        public async Task<QuestionViewModel> AnswerQuestionAndGetSolutionAsync(QuestionAnswerViewModel answer)
        {
            int questionId = await _context.CourseInstancesQuestion.Where(x => x.Id == answer.IdCourseInstanceQuestion)
                .Select(x => x.Question.Id).SingleAsync();

            CourseInstanceQuestion courseInstanceQuestion = await _context.FindAsync<CourseInstanceQuestion>(answer.IdCourseInstanceQuestion);
            courseInstanceQuestion.Correct = false;
            await _context.SaveChangesCranAsync(_currentPrincipal);
            
            return await GetQuestionAsync(questionId);
        }

        public async Task<QuestionResultViewModel> AnswerQuestionAndGetNextQuestionIdAsync(QuestionAnswerViewModel answer)
        {
            CourseInstanceQuestion courseInstanceQuestionEntity = await _context.FindAsync<CourseInstanceQuestion>(answer.IdCourseInstanceQuestion);
            CourseInstance courseInstanceEntity = await _context.FindAsync<CourseInstance>(courseInstanceQuestionEntity.IdCourseInstance);

            //Antworten abspeichern
            IList<CourseInstanceQuestionOption> options = await _context.CourseInstancesQuestionOption
                .Where(x => x.CourseInstanceQuestion.Id == courseInstanceQuestionEntity.Id)
                .OrderBy(x => x.Id)
                .Include(x => x.QuestionOption).ToListAsync();
            if(options.Count != answer.Answers.Count)
            {
                throw new InvalidOperationException("something wrong");
            }
            for(int i =0; i< options.Count; i++)
            {
                options[i].Checked = answer.Answers[i];
                options[i].Correct = options[i].QuestionOption.IsTrue == answer.Answers[i];
            }
            courseInstanceQuestionEntity.Correct = options.All(x => x.Correct);
            courseInstanceQuestionEntity.AnsweredAt = DateTime.Now;

            await _context.SaveChangesCranAsync(_currentPrincipal);

            //Nächste Frage vorbereiten.
            var sdfsd = await this.GetNextQuestion(courseInstanceEntity);
            QuestionResultViewModel result = new QuestionResultViewModel();
            result.IdCourseInstanceQuestionNext = sdfsd.IdCourseInstanceQuestion;
            result.AnsweredCorrectly = courseInstanceQuestionEntity.Correct;
            return result;
        }
    }
}
