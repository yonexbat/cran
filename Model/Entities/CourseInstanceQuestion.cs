using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Entities
{
    public class CourseInstanceQuestion : CranEntity
    {
        public virtual int IdCourseInstance { get; set; }
        public virtual int IdQuestion { get; set; }
        public virtual Question Question { get; set; }
        public virtual CourseInstance CourseInstance { get; set; }

        public virtual IList<CourseInstanceQuestionOption> CourseInstancesQuestionOption { get; set; } = new List<CourseInstanceQuestionOption>();
    }
}
