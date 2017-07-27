using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using cran.Model.ViewModel;
using Microsoft.AspNetCore.Authorization;

namespace cran.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class DataController : Controller
    {
        /// <summary>
        /// URL: http://localhost:5000/api/Data/Courses
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public CoursesList Courses()
        {
            CoursesList result = new CoursesList();
            result.Courses.Add(new Course
            {
                Title = "Javascript",
                Description = "Einfacher Javascript Kurs",
            });
            result.Courses.Add(new Course
            {
                Title = ".net Core",
                Description = "Dotnet Core Kurs",
            });

            return result;
        }
       
    }
}
