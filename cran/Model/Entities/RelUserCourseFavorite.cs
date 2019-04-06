using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Entities
{
    public class RelUserCourseFavorite : CranEntity
    {
        public virtual CranUser User { get; set; }
        public virtual Course Course { get; set; }
        public virtual int IdUser { get; set; }
        public virtual int IdCourse { get; set; }
    }
}
