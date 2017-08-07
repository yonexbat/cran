using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.ViewModel
{
    public class CourseInstanceViewModel
    {
        public int IdCourse { get; set; }
        public int IdCourseInstance { get; set; }
        public int IdCourseInstanceQuestion { get; set; }
        public int NumQuestionsTotal { get; set; }
        public int NumQuestionsAlreadyAsked { get; set; }
        public bool Done { get; set; }
    }
}
