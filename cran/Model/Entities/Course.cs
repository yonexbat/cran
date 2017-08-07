using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Entities
{
    public class Course : CranEntity
    {
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual int NumQuestionsToAsk { get;set;}

        public virtual IList<RelCourseTag> RelTags { get; set; } = new List<RelCourseTag>();
        public virtual IList<CourseInstance> CourseInstances { get; set; } = new List<CourseInstance>();
    }
}
