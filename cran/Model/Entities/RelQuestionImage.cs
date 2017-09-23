using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Entities
{
    public class RelQuestionImage : CranEntity
    {
        public virtual int IdQuestion { get; set; }
        public virtual int IdImage { get; set; }
        public virtual Image Image {get;set;}
        public virtual Question Question { get; set; }
    }
}
