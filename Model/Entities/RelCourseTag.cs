using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Entities
{
    public class RelCourseTag : CranEntity
    {
        public virtual int IdCourse { get; set; }
        public virtual int IdTag { get; set; }
        public virtual Tag Tag { get; set; }
        public virtual Course Course { get; set; }

    }
}
