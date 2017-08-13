using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using cran.Model.ViewModel;
using Microsoft.AspNetCore.Authorization;
using cran.Data;
using cran.Model.Entities;
using Microsoft.EntityFrameworkCore;
using cran.Services;
using cran.Filters;

namespace cran.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class DataController : Controller
    {

        private readonly IDbLogService _logService;
        private readonly ICraniumService _craninumService;

        public DataController(ICraniumService craniumService, IDbLogService logService)
        {
            _craninumService = craniumService;
            _logService = logService;
        }


        /// <summary>
        /// URL: http://localhost:5000/api/Data/Courses
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<CoursesListViewModel> Courses()
        {
            return await _craninumService.CoursesAsync();
        }

        /// <summary>
        /// URL: http://localhost:5000/api/Data/Question/3
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("[action]/{id?}")]
        public async Task<QuestionViewModel> Question(int id)
        {
            return await _craninumService.GetQuestionAsync(id);
        }


        /// <summary>
        /// URL: http://localhost:5000/api/Data/QuestionToAsk/2
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("[action]/{id}")]
        public async Task<QuestionToAskViewModel> QuestionToAsk(int id)
        {
            return await _craninumService.GetQuestionToAskAsync(id);
        }

        /// <summary>
        /// URL: http://localhost:5000/api/Data/QuestionToAsk/2
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<QuestionViewModel> AnswerQuestionAndGetSolution([FromBody] QuestionAnswerViewModel vm)
        {
            return await _craninumService.AnswerQuestionAndGetSolutionAsync(vm);
        }

        /// <summary>
        /// URL: http://localhost:5000/api/Data/FindTags?searchTerm=Hello
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IList<TagViewModel>> FindTags(string searchTerm)
        {
            return await _craninumService.FindTagsAsync(searchTerm);
        }

        /// <summary>
        /// URL: http://localhost:5000/api/Data/AddQuestion
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [ValidateModel]
        public async Task<InsertActionViewModel> AddQuestion([FromBody] QuestionViewModel vm)
        {
            return await _craninumService.AddQuestionAsync(vm); 
        }

        /// <summary>
        /// URL: http://localhost:5000/api/Data/AnswerQuestion
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [ValidateModel]
        public async Task<QuestionResultViewModel> AnswerQuestionAndGetNextQuestion([FromBody] QuestionAnswerViewModel vm)
        {
            return await _craninumService.AnswerQuestionAndGetNextQuestionIdAsync(vm);            
        }

        [HttpPost("[action]")]
        public async Task<CourseInstanceViewModel> StartCourse([FromBody] StartCourseViewModel vm)
        {
            return await _craninumService.StartCourseAsync(vm.IdCourse);
        }

        /// <summary>
        /// URL: http://localhost:5000/api/Data/SaveQuestion
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [ValidateModel]
        public async Task SaveQuestion([FromBody] QuestionViewModel vm)
        {
            await _craninumService.SaveQuestionAsync(vm);
        }


    }
}
