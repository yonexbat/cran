using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Dto
{
    public class RelCourseTagDto : IDto
    {
        public int Id { get; set; }
        public int IdTag { get; set; }
        public int IdCourse { get; set; }
    }
}
