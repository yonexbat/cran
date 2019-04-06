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
using System;
using cran.Model.Dto.Notification;

namespace cran.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [AutoValidateAntiforgeryToken]
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
        private readonly ITextService _textService;
        private readonly IExportService _exportService;
        private readonly IVersionService _versionService;
        private readonly INotificationService _notificationService;
        private readonly IFavoriteService _favoriteService;
        private const string OkReturnString = "Ok";

        public DataController(ISecurityService securityService,
            IBinaryService binaryService,
            IQuestionService questionService,
            IVersionService versionService,
            ITagService tagService,
            ICourseService courseService,
            ICommentsService commentsService,
            IUserProfileService userProfileService,
            ICourseInstanceService courseInstanceService,
            ITextService textService,
            IExportService exportService,
            INotificationService notificationService,
            IFavoriteService favoriteService)
        {
            _securityService = securityService;
            _binaryService = binaryService;
            _questionService = questionService;
            _tagService = tagService;
            _courseService = courseService;
            _commentsService = commentsService;
            _userProfileService = userProfileService;
            _courseInstanceService = courseInstanceService;
            _textService = textService;
            _exportService = exportService;
            _versionService = versionService;
            _notificationService = notificationService;
            _favoriteService = favoriteService;
        }

        #region QuestionService

        [HttpGet("[action]/{page}")]
        public async Task<PagedResultDto<QuestionListEntryDto>> GetMyQuestions(int page)
        {
            return await _questionService.GetMyQuestionsAsync(page);
        }

        [HttpDelete("[action]/{id}")]
        public async Task<JsonResult> DeleteQuestion(int id)
        {
            await _questionService.DeleteQuestionAsync(id);
            return Json(OkReturnString);
        }

        [HttpGet("[action]/{id}")]
        public async Task<QuestionDto> GetQuestion(int id)
        {
            return await _questionService.GetQuestionAsync(id);
        }

        [HttpPost("[action]")]
        [ValidateModel]
        public async Task<int> InsertQuestion([FromBody] QuestionDto vm)
        {
            return await _questionService.InsertQuestionAsync(vm);
        }

        [HttpPost("[action]")]
        [ValidateModel]
        public async Task<JsonResult> UpdateQuestion([FromBody] QuestionDto vm)
        {
            await _questionService.UpdateQuestionAsync(vm);
            return Json(OkReturnString);
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
        [Authorize(Roles = Roles.Admin)]
        public async Task<int> InsertTag([FromBody] TagDto vm)
        {
            return await _tagService.InsertTagAsync(vm);
        }

        [HttpPost("[action]")]
        [ValidateModel]
        [Authorize(Roles = Roles.Admin)]
        public async Task<JsonResult> UpdateTag([FromBody] TagDto vm)
        {
            await _tagService.UpdateTagAsync(vm);
            return Json(OkReturnString);
        }

        [HttpPost("[action]")]
        [ValidateModel]
        [Authorize(Roles = Roles.Admin)]
        public async Task<PagedResultDto<TagDto>> SearchForTags([FromBody]  SearchTags parameters)
        {
            return await _tagService.SearchForTagsAsync(parameters);
        }

        [HttpPost("[action]")]
        [ValidateModel]
        public async Task<IList<TagDto>> GetTags([FromBody]  IList<int> ids)
        {
            return await _tagService.GetTagsAsync(ids);
        }

        [HttpDelete("[action]/{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<JsonResult> DeleteTag(int id)
        {
            await _tagService.DeleteTagAsync(id);
            return Json(OkReturnString);
        }

        #endregion

        #region CourseService

        [HttpGet("[action]/{page}")]
        public async Task<PagedResultDto<CourseDto>> GetCourses(int page)
        {
            return await _courseService.GetCoursesAsync(page);
        }

        [HttpGet("[action]/{id}")]
        public async Task<CourseDto> GetCourse(int id)
        {
            return await _courseService.GetCourseAsync(id);
        }

        [HttpPost("[action]")]
        [ValidateModel]
        [Authorize(Roles = Roles.Admin)]
        public async Task<int> InsertCourse([FromBody] CourseDto vm)
        {
            return await _courseService.InsertCourseAsync(vm);
        }

        [HttpPost("[action]")]
        [ValidateModel]
        [Authorize(Roles = Roles.Admin)]
        public async Task<JsonResult> UpdateCourse([FromBody] CourseDto vm)
        {
            await _courseService.UpdateCourseAsync(vm);
            return Json(OkReturnString);
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
            return Json(OkReturnString);
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
        public async Task<JsonResult> AnswerQuestion([FromBody] QuestionAnswerDto vm)
        {
            await _courseInstanceService.AnswerQuestionAsync(vm);
            return Json(OkReturnString);
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
        [ValidateModel]
        public async Task<CourseInstanceDto> StartCourse([FromBody] StartCourseViewModel vm)
        {
            return await _courseInstanceService.StartCourseAsync(vm.IdCourse);
        }

        [HttpGet("[action]/{id}")]
        public async Task<ResultDto> GetCourseResult(int id)
        {
            return await _courseInstanceService.GetCourseResultAsync(id);
        }    

        [HttpGet("[action]/{page}")]
        public async Task<PagedResultDto<CourseInstanceListEntryDto>> GetMyCourseInstances(int page)
        {
            return await _courseInstanceService.GetMyCourseInstancesAsync(page);
        }

        [HttpDelete("[action]/{id}")]
        public async Task<JsonResult> DeleteCourseInstance(int id)
        {
            await _courseInstanceService.DeleteCourseInstanceAsync(id);
            return Json(OkReturnString);
        }

        #endregion

        #region SecurityService

        [HttpGet("[action]")]
        public IList<string> GetRolesOfUser()
        {
            return _securityService.GetRolesOfUser();
        }

        #endregion

        #region TextService

        [HttpGet("[action]/{id}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<TextDto> GetTextDto(int id)
        {
            return await _textService.GetTextDtoAsync(id);
        }

        [HttpGet("[action]/{key}")]        
        public async Task<TextDto> GetTextDtoByKey(string key)
        {
            return await _textService.GetTextDtoByKeyAsync(key);
        }

        [HttpPost("[action]")]
        [ValidateModel]
        [Authorize(Roles = Roles.Admin)]
        public async Task<JsonResult> UpdateText([FromBody]TextDto dto)
        {
            await _textService.UpdateTextAsync(dto);
            return Json(OkReturnString);
        }

        [HttpPost("[action]")]
        [ValidateModel]
        [Authorize(Roles = Roles.Admin)]
        public async Task<PagedResultDto<TextDto>> GetTexts([FromBody]SearchTextDto search)
        {
            return await _textService.GetTextsAsync(search);
        }



        #endregion

        #region ExportService
        [HttpGet("[action]")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Export()
        {
            Stream stream = await _exportService.Export();
            return File(stream, "application/zip", $"export_{DateTime.Now.ToString("yyyy_MM_dd_HH:mm")}.zip");
        }
        #endregion

        #region VersionService
        [HttpPost("[action]")]
        public async Task<int> CopyQuestion([FromBody] int id)
        {
            return await _versionService.CopyQuestionAsync(id);
        }

        [HttpPost("[action]")]
        public async Task<int> VersionQuestion([FromBody]int id)
        {
            return await _versionService.VersionQuestionAsync(id);
        }

        [HttpPost("[action]")]
        public async Task<JsonResult> AcceptQuestion([FromBody]int id)
        {
            await _versionService.AcceptQuestionAsync(id);
            return Json(OkReturnString);
        }

        [HttpPost("[action]")]
        public async Task<PagedResultDto<VersionInfoDto>> GetVersions([FromBody]VersionInfoParametersDto versionInfoParameters)
        {
            return await _versionService.GetVersionsAsync(versionInfoParameters);            
        }

        #endregion

        #region NotificationService
        [HttpPost("[action]")]
        public async Task<JsonResult> AddPushRegistration([FromBody]NotificationSubscriptionDto dto)
        {
            await _notificationService.AddPushNotificationSubscriptionAsync(dto);
            return Json(OkReturnString);
        }

        [HttpPost("[action]")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<JsonResult> SendNotificationToUser([FromBody]NotificationDto dto)
        {
            await _notificationService.SendNotificationToUserAsync(dto);
            return Json(OkReturnString);
        }

        [HttpGet("[action]/{page}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<PagedResultDto<SubscriptionShortDto>> GetAllSubscriptions(int page)
        {
            return await _notificationService.GetAllSubscriptionsAsync(page);
        }

        #endregion

        #region FavoriteService
        [HttpPost("[action]")]
        public async Task<JsonResult> AddCourseToFavorites([FromBody]CourseToFavoritesDto dto)
        {
            await _favoriteService.AddCourseToFavoritesAsync(dto);
            return Json(OkReturnString);
        }

        [HttpPost("[action]")]
        public async Task<JsonResult> RemoveCoureFromFavorites([FromBody]CourseToFavoritesDto dto)
        {
            await _favoriteService.RemoveCoureFromFavoritesAsync(dto);
            return Json(OkReturnString);
        }

        [HttpGet("[action]/{page}")]
        public async Task<PagedResultDto<CourseDto>> GetFavoriteCourses(int page)
        {
            return await _favoriteService.GetFavoriteCourseAsync(page);
        }
        #endregion
    }
}
