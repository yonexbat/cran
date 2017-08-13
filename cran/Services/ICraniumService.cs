using cran.Model.Dto;
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
        Task<InsertActionDto> AddQuestionAsync(QuestionDto question);

        Task SaveQuestionAsync(QuestionDto question);

        Task DeleteQuestionAsync(int idQuestion);

        Task<CoursesListDto> CoursesAsync();

        Task<IList<QuestionListEntryDto>> GetMyQuestionsAsync();

        Task<QuestionDto> GetQuestionAsync(int id);

        Task<IList<TagDto>> FindTagsAsync(string searchTerm);

        Task<CourseInstanceDto> StartCourseAsync(int idCourse);

        Task<CourseInstanceDto> NextQuestion(int courseInstanceId);

        Task<QuestionToAskDto> GetQuestionToAskAsync(int courseInstanceQuestionId);

        Task<QuestionDto> AnswerQuestionAndGetSolutionAsync(QuestionAnswerDto answer);

        Task<CourseInstanceDto> AnswerQuestionAndGetNextQuestionAsync(QuestionAnswerDto answer);
    }
}
