using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using cran.Model.ViewModel;
using Microsoft.AspNetCore.Authorization;
using cran.Services;
using cran.Filters;
using cran.Model.Dto;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace cran.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class DataController : Controller
    {

        private readonly ISecurityService _securityService;
        private readonly IBinaryService _binaryService;
        private readonly IQuestionService _questionService;
        private readonly ITagService _tagService;
        private readonly ICourseService _courseService;
        private readonly ICommentsService _commentsService;
        private readonly IUserProfileService _userProfileService;
        private readonly ICourseInstanceService _courseInstanceService;
        private const string Ok = "Ok";

        public DataController(ISecurityService securityService,
            IBinaryService binaryService,
            IQuestionService questionService,
            ITagService tagService,
            ICourseService courseService,
            ICommentsService commentsService,
            IUserProfileService userProfileService,
            ICourseInstanceService courseInstanceService)
        {
            _securityService = securityService;
            _binaryService = binaryService;
            _questionService = questionService;
            _tagService = tagService;
            _courseService = courseService;
            _commentsService = commentsService;
            _userProfileService = userProfileService;
            _courseInstanceService = courseInstanceService;
        }

        #region QuestionService

        [HttpGet("[action]")]
        public async Task<IList<QuestionListEntryDto>> GetMyQuestions()
        {
            return await _questionService.GetMyQuestionsAsync();
        }

        [HttpDelete("[action]/{id}")]
        public async Task DeleteQuestion(int id)
        {
            await _questionService.DeleteQuestionAsync(id);
        }


        [HttpGet("[action]/{id}")]
        public async Task<QuestionDto> GetQuestion(int id)
        {
            return await _questionService.GetQuestionAsync(id);
        }

        [HttpPost("[action]")]
        [ValidateModel]
        public async Task<InsertActionDto> InsertQuestion([FromBody] QuestionDto vm)
        {
            return await _questionService.InsertQuestionAsync(vm);
        }

        [HttpPost("[action]")]
        [ValidateModel]
        public async Task<JsonResult> UpdateQuestion([FromBody] QuestionDto vm)
        {
            await _questionService.UpdateQuestionAsync(vm);
            return Json(Ok);
        }

        [HttpPost("[action]")]
        [ValidateModel]
        public async Task<PagedResultDto<QuestionListEntryDto>> SearchForQuestions([FromBody]  SearchQParametersDto parameters)
        {
            return await _questionService.SearchForQuestionsAsync(parameters);
        }

        [HttpPost("[action]")]
        public async Task<ImageDto> AddImage([FromBody] ImageDto image)
        {
            return await _questionService.AddImageAsync(image);
        }

        #endregion

        #region TagService

        [HttpGet("[action]/{id}")]
        public async Task<TagDto> GetTag(int id)
        {
            return await _tagService.GetTagAsync(id);
        }

        [HttpGet("[action]")]
        public async Task<IList<TagDto>> FindTags(string searchTerm)
        {
            return await _tagService.FindTagsAsync(searchTerm);
        }

        [HttpPost("[action]")]
        [ValidateModel]
        public async Task<InsertActionDto> InsertTag([FromBody] TagDto vm)
        {
            return await _tagService.InsertTagAsync(vm);
        }

        [HttpPost("[action]")]
        [ValidateModel]
        public async Task<JsonResult> UpdateTag([FromBody] TagDto vm)
        {
            await _tagService.UpdateTagAsync(vm);
            return Json(Ok);
        }

        [HttpPost("[action]")]
        [ValidateModel]
        public async Task<PagedResultDto<TagDto>> SearchForTags([FromBody]  SearchTags parameters)
        {
            return await _tagService.SearchForTags(parameters);
        }

        [HttpDelete("[action]/{id}")]
        public async Task<JsonResult> DeleteTag(int id)
        {
            await _tagService.DeleteTagAsync(id);
            return Json(Ok);
        }

        #endregion

        #region CourseService

        [HttpGet("[action]")]
        public async Task<CoursesDto> GetCourses()
        {
            return await _courseService.GetCoursesAsync();
        }

        [HttpGet("[action]/{id}")]
        public async Task<CourseDto> GetCourse(int id)
        {
            return await _courseService.GetCourseAsync(id);
        }

        [HttpPost("[action]")]
        [ValidateModel]
        public async Task<InsertActionDto> InsertCourse([FromBody] CourseDto vm)
        {
            return await _courseService.InsertCourseAsync(vm);
        }

        [HttpPost("[action]")]
        [ValidateModel]
        public async Task<JsonResult> UpdateCourse([FromBody] CourseDto vm)
        {
            await _courseService.UpdateCourseAsync(vm);
            return Json(Ok);
        }

        #endregion

        #region CommentsService

        [HttpPost("[action]")]
        [ValidateModel]
        public async Task<int> AddComment([FromBody] CommentDto vm)
        {
            return await _commentsService.AddCommentAsync(vm);
        }

        [HttpPost("[action]")]
        [ValidateModel]
        public async Task<PagedResultDto<CommentDto>> GetComments([FromBody]  GetCommentsDto parameters)
        {
            return await _commentsService.GetCommentssAsync(parameters);
        }

        [HttpDelete("[action]/{id}")]
        public async Task<JsonResult> DeleteComment(int id)
        {
            await _commentsService.DeleteCommentAsync(id);
            return Json(Ok);
        }

        [HttpPost("[action]")]
        public async Task<VotesDto> Vote([FromBody] VotesDto vote)
        {
            return await _commentsService.VoteAsync(vote);
        }
        #endregion

        #region UserProfileService

        [HttpGet("[action]")]
        public async Task<UserInfoDto> GetUserInfo()
        {
            return await _userProfileService.GetUserInfoAsync();
        }

        #endregion

        #region Binaryservice

        [HttpPost("[action]")]
        public async Task<IList<BinaryDto>> UploadFiles(List<IFormFile> files)
        {
            return await _binaryService.UploadFilesAsync(files);
        }

        [HttpGet("[action]/{id}")]
        public async Task<FileStreamResult> GetFile(int id)
        {
            BinaryDto fileInfo = await _binaryService.GetFileInfoAsync(id);
            Stream stream = await _binaryService.GetBinaryAsync(id);
            var result = File(stream, fileInfo.ContentType, fileInfo.FileName);
            return result;
        }

        #endregion

        #region CourseInstanceService

        [HttpGet("[action]/{id}")]
        public async Task<QuestionToAskDto> GetQuestionToAsk(int id)
        {
            return await _courseInstanceService.GetQuestionToAskAsync(id);
        }

        [HttpPost("[action]")]
        public async Task<QuestionDto> AnswerQuestionAndGetSolution([FromBody] QuestionAnswerDto vm)
        {
            return await _courseInstanceService.AnswerQuestionAndGetSolutionAsync(vm);
        }

        [HttpPost("[action]")]
        [ValidateModel]
        public async Task<CourseInstanceDto> AnswerQuestionAndGetNextQuestion([FromBody] QuestionAnswerDto vm)
        {
            return await _courseInstanceService.AnswerQuestionAndGetNextQuestionAsync(vm);
        }

        [HttpPost("[action]")]
        public async Task<CourseInstanceDto> StartCourse([FromBody] StartCourseViewModel vm)
        {
            return await _courseInstanceService.StartCourseAsync(vm.IdCourse);
        }

        [HttpGet("[action]/{id}")]
        public async Task<ResultDto> GetCourseResult(int id)
        {
            return await _courseInstanceService.GetCourseResultAsync(id);
        }

        [HttpGet("[action]")]
        public async Task<IList<CourseInstanceListEntryDto>> GetMyCourseInstances()
        {
            return await _courseInstanceService.GetMyCourseInstancesAsync();
        }

        [HttpDelete("[action]/{id}")]
        public async Task<JsonResult> DeleteCourseInstance(int id)
        {
            await _courseInstanceService.DeleteCourseInstanceAsync(id);
            return Json(Ok);
        }

        #endregion

        #region SecurityService

        [HttpGet("[action]")]
        public IList<string> GetRolesOfUser()
        {
            return _securityService.GetRolesOfUser();
        }
        
        #endregion



    }
}
