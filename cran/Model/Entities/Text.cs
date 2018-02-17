using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Entities
{
    public class Text : CranEntity
    {
        public virtual string Key { get; set; }
        public virtual string ContentDe { get; set; }
        public virtual string ContentEn { get; set; }
    }
}
