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

        //Course
        Task<CoursesDto> GetCoursesAsync();        
        Task<CourseDto> GetCourseAsync(int id);
        Task<InsertActionDto> InsertCourseAsync(CourseDto vm);
        Task UpdateCourseAsync(CourseDto vm);

        //CourseInstance
        Task<CourseInstanceDto> StartCourseAsync(int idCourse);
        Task<CourseInstanceDto> NextQuestion(int idCourseInstance);
        Task<QuestionDto> AnswerQuestionAndGetSolutionAsync(QuestionAnswerDto answer);
        Task<CourseInstanceDto> AnswerQuestionAndGetNextQuestionAsync(QuestionAnswerDto answer);
        Task<QuestionToAskDto> GetQuestionToAskAsync(int idCourseInstance);
        Task<ResultDto> GetCourseResultAsync(int idCourseInstance);
        Task<IList<CourseInstanceListEntryDto>> GetMyCourseInstancesAsync();
        Task DeleteCourseInstanceAsync(int idCourseInstance);


        //Tags
        Task<TagDto> GetTagAsync(int id);
        Task UpdateTagAsync(TagDto vm);
        Task<InsertActionDto> InsertTagAsync(TagDto vm);
        Task<IList<TagDto>> FindTagsAsync(string searchTerm);               
        Task<PagedResultDto<TagDto>> SearchForTags(SearchTags parameters);

        //comments and voting
        Task<PagedResultDto<CommentDto>> GetCommentssAsync(GetCommentsDto parameters);
        Task<int> AddCommentAsync(CommentDto vm);
        Task DeleteCommentAsync(int id);
        Task<VotesDto> VoteAsync(VotesDto vote);

        //Security      
        Task<UserInfoDto> GetUserInfoAsync();
     }
}
