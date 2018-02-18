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
        private ITextService _textService;

        public CourseController(ICourseService courseService, 
            ITextService textService)
        {
            _courseService = courseService;
            _textService = textService;
        }

        public async Task<IActionResult> Index(int id)
        {
            CourseViewModel vm = new CourseViewModel();
            await InitCourse(id, vm);
            return View(vm);
        }

        public async Task<IActionResult> Info(string id)
        {
            InfoViewModel vm = new InfoViewModel();
            vm.InfoText = await _textService.GetTextAsync(id);
            return View(vm);
        }

        private async Task InitCourse(int id, CourseViewModel viewModel)
        {
            CourseDto  course = await _courseService.GetCourseAsync(id);
            viewModel.Course = course;
            viewModel.Id = id;
           
        }

        public IActionResult Start(CourseViewModel vm)
        {
            string url = $"~/jsclient/coursestarter/{vm.Id}";
            return Redirect(url);
        }
    }
}
