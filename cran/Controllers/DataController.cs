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

        public DataController(ICraniumService craniumService)
        {
            _craninumService = craniumService;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<CoursesListDto> GetCourses()
        {
            return await _craninumService.CoursesAsync();
        }


        /// <summary>
        /// URL: http://localhost:5000/api/Data/GetMyQuestions
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IList<QuestionListEntryDto>> GetMyQuestions()
        {
            return await _craninumService.GetMyQuestionsAsync();
        }

        /// <summary>
        ///  URL: http://localhost:5000/api/Data/DeleteQuestion
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("[action]/{id}")]
        public async Task DeleteQuestion(int id)
        {
            await _craninumService.DeleteQuestionAsync(id);
        }

        /// <summary>
        /// URL: http://localhost:5000/api/Data/GetQuestion/3
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("[action]/{id}")]
        public async Task<QuestionDto> GetQuestion(int id)
        {
            return await _craninumService.GetQuestionAsync(id);
        }


        /// <summary>
        /// URL: http://localhost:5000/api/Data/QuestionToAsk/2
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("[action]/{id}")]
        public async Task<QuestionToAskDto> GetQuestionToAsk(int id)
        {
            return await _craninumService.GetQuestionToAskAsync(id);
        }

        /// <summary>
        /// URL: http://localhost:5000/api/Data/QuestionToAsk/2
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<QuestionDto> AnswerQuestionAndGetSolution([FromBody] QuestionAnswerDto vm)
        {
            return await _craninumService.AnswerQuestionAndGetSolutionAsync(vm);
        }

        /// <summary>
        /// URL: http://localhost:5000/api/Data/FindTags?searchTerm=Hello
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IList<TagDto>> FindTags(string searchTerm)
        {
            return await _craninumService.FindTagsAsync(searchTerm);
        }

        /// <summary>
        /// URL: http://localhost:5000/api/Data/InsertQuestion
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [ValidateModel]
        public async Task<InsertActionDto> InsertQuestion([FromBody] QuestionDto vm)
        {
            return await _craninumService.InsertQuestionAsync(vm);
        }

        /// <summary>
        /// URL: http://localhost:5000/api/Data/AnswerQuestion
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
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

        /// <summary>
        ///
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
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


    }
}
