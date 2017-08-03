using cran.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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

        public async Task<int> AddQuestionAsync(QuestionViewModel question)
        {
            Question questionEntity = new Question
            {
                Title = question.Title,
                Text = question.Text,               
            };

            InitTechnicalFields(questionEntity);

            await _dbLogService.LogMessageAsync("Adding question");
            _context.Questions.Add(questionEntity);
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

        public async Task<QuestionViewModel> GetQuestionAsync(int id)
        {
            Question question = await _context.FindAsync<Question>(id);
            QuestionViewModel vm = new QuestionViewModel
            {
                Id = question.Id,
                Text = question.Text,
                Title = question.Title,       
            };

            foreach(QuestionOption option in _context.QuestionOptions.Where(x => x.IdQuestion == id))
            {
                vm.Options.Add(new QuestionOptionViewModel
                {
                    Id = option.Id,
                    IsTrue = option.IsTrue,
                    Text = option.Text,
                });
            }

            return vm;
        }

 

        public async Task UpdateQuestionAsync(QuestionViewModel question)
        {
            Question questionEntity = await _context.FindAsync<Question>(question.Id);
            questionEntity.Title = question.Title;
            questionEntity.Text = question.Text;

            foreach(QuestionOption optionEntity in questionEntity.Options)
            {
                _context.QuestionOptions.Remove(optionEntity);
            }

            foreach(QuestionOptionViewModel option in question.Options)
            {
                QuestionOption optionEntity = new QuestionOption();
                optionEntity.Question = questionEntity;
                optionEntity.IsTrue = option.IsTrue;
                optionEntity.Text = option.Text;

                InitTechnicalFields(optionEntity);
                questionEntity.Options.Add(optionEntity);
                _context.QuestionOptions.Add(optionEntity);
            }

            await _context.SaveChangesAsync();

        }

    
        
    }
}
