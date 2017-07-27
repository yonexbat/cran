using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.ViewModel
{
    public class CoursesList : Base
    {
        public IList<Course> Courses { get; set; } = new List<Course>();
    }
}
