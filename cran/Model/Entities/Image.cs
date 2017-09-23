using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Entities
{
    public class Image : CranEntity
    {
        public virtual int IdBinary { get; set; }
        public virtual int? Width { get; set; }
        public virtual int? Height { get; set; }
        public virtual bool Full { get; set; }

        public virtual Binary Binary { get; set; }
        public virtual RelQuestionImage RelImage { get; set; }
  
    }
}
