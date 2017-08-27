using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Dto
{
    public class CourseInstanceListEntryDto
    {      
        public int IdCourseInstance { get; set; }
        public string Title { get; set; }
        public int Percentage { get; set; }
        public DateTime InsertDate { get; set; }
    }
}
