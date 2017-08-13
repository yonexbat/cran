using cran.Model.Entities;
using cran.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Services
{
    public interface ICraniumService
    {
        Task<InsertActionViewModel> AddQuestionAsync(QuestionViewModel question);

        Task SaveQuestionAsync(QuestionViewModel question);

        Task<CoursesListViewModel> CoursesAsync();

        Task<QuestionViewModel> GetQuestionAsync(int id);

        Task<IList<TagViewModel>> FindTagsAsync(string searchTerm);

        Task<CourseInstanceViewModel> StartCourseAsync(int courseId);

        Task<CourseInstanceViewModel> NextQuestion(int courseInstanceId);

        Task<QuestionToAskViewModel> GetQuestionToAskAsync(int courseInstanceQuestionId);

        Task<QuestionViewModel> AnswerQuestionAndGetSolutionAsync(QuestionAnswerViewModel answer);

        Task<QuestionResultViewModel> AnswerQuestionAndGetNextQuestionIdAsync(QuestionAnswerViewModel answer);
    }
}
