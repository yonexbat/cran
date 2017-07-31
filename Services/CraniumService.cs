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
    public class CraniumService : ICraniumService
    {
        private ApplicationDbContext _context;

        private IDbLogService _dbLogService;

        private IPrincipal _currentPrincipal;

        public CraniumService(ApplicationDbContext context, IDbLogService dbLogService, IPrincipal principal)
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
                InsertDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                InsertUser = GetUserId(),
                UpdateUser = GetUserId(),
            };

            await _dbLogService.LogMessageAsync("Adding question");
            _context.Questions.Add(questionEntity);
            await _context.SaveChangesAsync();
            return questionEntity.Id;
        }

        public async Task<CoursesListViewModel> CoursesAsync()
        {
            await _dbLogService.LogMessageAsync("courses");
            CoursesListViewModel result = new CoursesListViewModel();
            IList<Course> list = await this._context.Courses.ToListAsync();
            foreach (Course course in list)
            {
                result.Courses.Add(new CourseViewModel
                {
                    Id = course.Id,
                    Title = course.Title,
                    Description = course.Description,
                });
            }

            return result;
        }

        public async Task<QuestionViewModel> GetQuestionAsync(int id)
        {
            Question question = await _context.FindAsync<Question>(id);
            return new QuestionViewModel
            {
                Id = question.Id,
                Text = question.Text,
                Title = question.Title,
            };
        }

        public string GetUserId()
        {
            return _currentPrincipal.Identity.Name;
        }
    }
}
