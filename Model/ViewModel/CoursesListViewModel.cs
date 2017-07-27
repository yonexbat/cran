using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.ViewModel
{
    public class CoursesListViewModel : BaseViewModel
    {
        public IList<CourseViewModel> Courses { get; set; } = new List<CourseViewModel>();
    }
}
