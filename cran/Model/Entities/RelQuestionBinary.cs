using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Entities
{
    public class RelQuestionBinary : CranEntity
    {
        public virtual int IdQuestion { get; set; }
        public virtual int IdBinary { get; set; }
        public virtual Binary Binary { get; set; }
        public virtual Question Question { get; set; }
    }
}
