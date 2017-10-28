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
        Task<InsertActionDto> InsertQuestionAsync(QuestionDto question);

        Task UpdateQuestionAsync(QuestionDto question);

        Task DeleteQuestionAsync(int idQuestion);

        Task<CoursesDto> GetCoursesAsync();

        Task<IList<QuestionListEntryDto>> GetMyQuestionsAsync();

        Task<QuestionDto> GetQuestionAsync(int id);

        Task<CourseDto> GetCourseAsync(int id);

        Task<InsertActionDto> InsertCourseAsync(CourseDto vm);

        Task UpdateCourseAsync(CourseDto vm);

        Task<TagDto> GetTagAsync(int id);

        Task UpdateTagAsync(TagDto vm);

        Task<InsertActionDto> InsertTagAsync(TagDto vm);

        Task<IList<TagDto>> FindTagsAsync(string searchTerm);

        Task<CourseInstanceDto> StartCourseAsync(int idCourse);

        Task<CourseInstanceDto> NextQuestion(int idCourseInstance);

        Task<QuestionToAskDto> GetQuestionToAskAsync(int idCourseInstance);

        Task<QuestionDto> AnswerQuestionAndGetSolutionAsync(QuestionAnswerDto answer);

        Task<CourseInstanceDto> AnswerQuestionAndGetNextQuestionAsync(QuestionAnswerDto answer);

        Task<ResultDto> GetCourseResultAsync(int idCourseInstance);

        Task<IList<CourseInstanceListEntryDto>> GetMyCourseInstancesAsync();

        Task DeleteCourseInstanceAsync(int idCourseInstance);

        Task<PagedResultDto<QuestionListEntryDto>> SearchForQuestionsAsync(SearchQParametersDto parameters);

        Task<PagedResultDto<TagDto>> SearchForTags(SearchTags parameters);

        Task<PagedResultDto<CommentDto>> GetCommentssAsync(GetCommentsDto parameters);

        Task<int> AddCommentAsync(CommentDto vm);

        Task DeleteCommentAsync(int id);

        Task<VotesDto> VoteAsync(VotesDto vote);

        Task<ImageDto> AddImageAsync(ImageDto imageDto);

        Task<UserInfoDto> GetUserInfoAsync();
     }
}
