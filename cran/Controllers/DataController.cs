using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using cran.Model.ViewModel;
using Microsoft.AspNetCore.Authorization;
using cran.Services;
using cran.Filters;
using cran.Model.Dto;

namespace cran.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class DataController : Controller
    {

        private readonly ICraniumService _craninumService;
        private readonly ISecurityService _securityService;

        public DataController(ICraniumService craniumService, ISecurityService securityService)
        {
            _craninumService = craniumService;
            _securityService = securityService;
        }


        [HttpGet("[action]")]
        public async Task<CoursesListDto> GetCourses()
        {
            return await _craninumService.CoursesAsync();
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

        [HttpGet("[action]")]
        public IList<string> GetRolesOfUser()
        {
            return _securityService.GetRolesOfUser();
        }

    }
}
