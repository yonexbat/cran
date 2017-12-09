using cran.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Services
{
    public interface ICourseInstanceService
    {
        Task<CourseInstanceDto> StartCourseAsync(int idCourse);
        Task<CourseInstanceDto> NextQuestion(int idCourseInstance);
        Task<QuestionDto> AnswerQuestionAndGetSolutionAsync(QuestionAnswerDto answer);
        Task<CourseInstanceDto> AnswerQuestionAndGetNextQuestionAsync(QuestionAnswerDto answer);
        Task<QuestionToAskDto> GetQuestionToAskAsync(int idCourseInstance);
        Task<ResultDto> GetCourseResultAsync(int idCourseInstance);
        Task<PagedResultDto<CourseInstanceListEntryDto>> GetMyCourseInstancesAsync(int page);
        Task DeleteCourseInstanceAsync(int idCourseInstance);
    }
}
