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


        public CraniumService(ApplicationDbContext context, IDbLogService dbLogService, IPrincipal principal) :
            base(context, principal)
        {
            _context = context;
            _dbLogService = dbLogService;
            _currentPrincipal = principal;
        }

        public async Task<int> AddQuestionAsync(QuestionViewModel questionVm)
        {
            Question questionEntity = new Question
            {
                Title = questionVm.Title,
                Text = questionVm.Text,               
            };

            InitTechnicalFields(questionEntity);           
            await _dbLogService.LogMessageAsync("Adding question");
            _context.Questions.Add(questionEntity);

            AddOptions(questionVm, questionEntity);
            await AddTags(questionVm, questionEntity);

            await _context.SaveChangesAsync();
            return questionEntity.Id;
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

 

        public async Task UpdateQuestionAsync(QuestionViewModel questionVm)
        {
            Question questionEntity = await _context.FindAsync<Question>(questionVm.Id);
            questionEntity.Title = questionVm.Title;
            questionEntity.Text = questionVm.Text;

            foreach(QuestionOption optionEntity in _context.QuestionOptions.Where(x => x.IdQuestion == questionEntity.Id))
            {                
                _context.Remove(optionEntity);
            }

            foreach(RelQuestionTag relTagEntity in _context.RelQuestionTags.Where(x => x.IdQuestion == questionEntity.Id))
            {
                _context.Remove(relTagEntity);
            }

            AddOptions(questionVm, questionEntity);
            await AddTags(questionVm, questionEntity);

            await _context.SaveChangesAsync();
        }

        private void AddOptions(QuestionViewModel questionVm, Question questionEntity)
        {
            foreach (QuestionOptionViewModel option in questionVm.Options)
            {
                QuestionOption optionEntity = new QuestionOption();
                optionEntity.Question = questionEntity;
                optionEntity.IsTrue = option.IsTrue;
                optionEntity.Text = option.Text;

                InitTechnicalFields(optionEntity);
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
                InitTechnicalFields(relTag);
                _context.RelQuestionTags.Add(relTag);
            }
        }
        
    }
}
