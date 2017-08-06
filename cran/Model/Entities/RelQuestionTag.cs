using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Entities
{
    public class RelQuestionTag : CranEntity
    {
        public virtual int IdQuestion { get; set; }
        public virtual int IdTag { get; set; }
        public virtual Tag Tag { get; set; }
        public virtual Question Question { get; set; }
    }
}
