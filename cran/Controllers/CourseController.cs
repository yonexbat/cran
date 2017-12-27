using cran.Model.Dto;
using cran.Model.ViewModel;
using cran.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Controllers
{
    public class CourseController : Controller
    {
        private ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public async Task<IActionResult> Index(int id)
        {
            CourseViewModel vm = new CourseViewModel();
            await InitCourse(id, vm);
            return View(vm);
        }

        private async Task InitCourse(int id, CourseViewModel viewModel)
        {
            CourseDto  course = await _courseService.GetCourseAsync(id);
            viewModel.Course = course;
            viewModel.Id = id;
           
        }

        public async Task<IActionResult> Start(CourseViewModel vm)
        {
            string url = $"~/jsclient/coursestarter/{vm.Id}";
            return Redirect(url);
        }
    }
}
