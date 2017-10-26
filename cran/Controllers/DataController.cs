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

        private readonly ICraniumService _craninumService;
        private readonly ISecurityService _securityService;
        private readonly IBinaryService _binaryService;

        public DataController(ICraniumService craniumService,
            ISecurityService securityService,
            IBinaryService binaryService)
        {
            _craninumService = craniumService;
            _securityService = securityService;
            _binaryService = binaryService;
        }


        [HttpGet("[action]")]
        public async Task<CoursesDto> GetCourses()
        {
            return await _craninumService.GetCoursesAsync();
        }


        [HttpGet("[action]")]
        public async Task<IList<QuestionListEntryDto>> GetMyQuestions()
        {
            return await _craninumService.GetMyQuestionsAsync();
        }


        [HttpDelete("[action]/{id}")]
        public async Task DeleteQuestion(int id)
        {
            await _craninumService.DeleteQuestionAsync(id);
        }


        [HttpGet("[action]/{id}")]
        public async Task<QuestionDto> GetQuestion(int id)
        {
            return await _craninumService.GetQuestionAsync(id);
        }

        [HttpGet("[action]/{id}")]
        public async Task<TagDto> GetTag(int id)
        {
            return await _craninumService.GetTagAsync(id);
        }


        [HttpGet("[action]/{id}")]
        public async Task<QuestionToAskDto> GetQuestionToAsk(int id)
        {
            return await _craninumService.GetQuestionToAskAsync(id);
        }

        [HttpPost("[action]")]
        public async Task<QuestionDto> AnswerQuestionAndGetSolution([FromBody] QuestionAnswerDto vm)
        {
            return await _craninumService.AnswerQuestionAndGetSolutionAsync(vm);
        }


        [HttpGet("[action]")]
        public async Task<IList<TagDto>> FindTags(string searchTerm)
        {
            return await _craninumService.FindTagsAsync(searchTerm);
        }


        [HttpPost("[action]")]
        [ValidateModel]
        public async Task<InsertActionDto> InsertQuestion([FromBody] QuestionDto vm)
        {
            return await _craninumService.InsertQuestionAsync(vm);
        }


        [HttpPost("[action]")]
        [ValidateModel]
        public async Task<InsertActionDto> InsertTag([FromBody] TagDto vm)
        {
            return await _craninumService.InsertTagAsync(vm);
        }


        [HttpPost("[action]")]
        [ValidateModel]
        public async Task<CourseInstanceDto> AnswerQuestionAndGetNextQuestion([FromBody] QuestionAnswerDto vm)
        {
            return await _craninumService.AnswerQuestionAndGetNextQuestionAsync(vm);
        }

        [HttpPost("[action]")]
        public async Task<CourseInstanceDto> StartCourse([FromBody] StartCourseViewModel vm)
        {
            return await _craninumService.StartCourseAsync(vm.IdCourse);
        }

        [HttpPost("[action]")]
        [ValidateModel]
        public async Task UpdateQuestion([FromBody] QuestionDto vm)
        {
            await _craninumService.UpdateQuestionAsync(vm);
        }

        [HttpPost("[action]")]
        [ValidateModel]
        public async Task UpdateTag([FromBody] TagDto vm)
        {
            await _craninumService.UpdateTagAsync(vm);
        }


        [HttpGet("[action]/{id}")]
        public async Task<ResultDto> GetCourseResult(int id)
        {
            return await _craninumService.GetCourseResultAsync(id);
        }

        [HttpGet("[action]")]
        public async Task<IList<CourseInstanceListEntryDto>> GetMyCourseInstances()
        {
            return await _craninumService.GetMyCourseInstancesAsync();
        }

        [HttpDelete("[action]/{id}")]
        public async Task DeleteCourseInstance(int id)
        {
            await _craninumService.DeleteCourseInstanceAsync(id);
        }

        [HttpPost("[action]")]
        [ValidateModel]
        public async Task<PagedResultDto<QuestionListEntryDto>> SearchForQuestions([FromBody]  SearchQParametersDto parameters)
        {
            return await _craninumService.SearchForQuestionsAsync(parameters);
        }

        [HttpPost("[action]")]
        [ValidateModel]
        public async Task<PagedResultDto<TagDto>> SearchForTags([FromBody]  SearchTags parameters)
        {
            return await _craninumService.SearchForTags(parameters);
        }

        [HttpGet("[action]")]
        public IList<string> GetRolesOfUser()
        {
            return _securityService.GetRolesOfUser();
        }

        [HttpPost("[action]")]
        [ValidateModel]
        public async Task<int> AddComment([FromBody] CommentDto vm)
        {
            return await _craninumService.AddCommentAsync(vm);
        }

        [HttpPost("[action]")]
        [ValidateModel]
        public async Task<PagedResultDto<CommentDto>> GetComments([FromBody]  GetCommentsDto parameters)
        {
            return await _craninumService.GetCommentssAsync(parameters);
        }

        [HttpDelete("[action]/{id}")]
        public async Task DeleteComment(int id)
        {
            await _craninumService.DeleteCommentAsync(id);
        }

        [HttpPost("[action]")]
        public async Task<VotesDto> Vote([FromBody] VotesDto vote)
        {
            return await _craninumService.VoteAsync(vote);
        }

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

        [HttpPost("[action]")]
        public async Task<ImageDto> AddImage([FromBody] ImageDto image)
        {
            return await _craninumService.AddImageAsync(image);
        }

        [HttpGet("[action]")]
        public async Task<UserInfoDto> GetUserInfo()
        {
            return await _craninumService.GetUserInfoAsync();
        }

    }
}
