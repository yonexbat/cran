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

namespace cran.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class DataController : Controller
    {

        private readonly ApplicationDbContext _dbContext;

        public DataController(ApplicationDbContext context)
        {
            _dbContext = context;
        }


        /// <summary>
        /// URL: http://localhost:5000/api/Data/Courses
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<CoursesListViewModel> Courses()
        {
            CoursesListViewModel result = new CoursesListViewModel();
            IList<Course> list = await this._dbContext.Courses.ToListAsync();
            foreach (Course course in list)
            {
                result.Courses.Add(new CourseViewModel
                {
                    Id = course.Id,
                    Title = course.Title,
                    Description = course.Description,
                });
            }        

            return result;
        }
       
    }
}
