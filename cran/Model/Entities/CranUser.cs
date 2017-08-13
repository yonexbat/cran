using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Entities
{
    public class CranUser : CranEntity
    {
        public virtual string UserId { get; set; }

        public virtual IList<CourseInstance> CourseInstances { get; set; } = new List<CourseInstance>();
        public virtual IList<Question> Questions { get; set; } = new List<Question>();
    }
}
