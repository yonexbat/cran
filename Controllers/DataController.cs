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
        /// URL: http://localhost:5000/api/Data/AddQuestion
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [ValidateModel]
        public async Task<InsertActionViewModel> AddQuestion([FromBody] QuestionViewModel vm)
        {

            int id = await _craninumService.AddQuestionAsync(vm);

            return new InsertActionViewModel
            {
                NewId = id,
                Status = "Ok",
            };
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

            await _craninumService.UpdateQuestionAsync(vm);
        }


    }
}
