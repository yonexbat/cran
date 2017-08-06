using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Entities
{
    public class Tag : CranEntity
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
    }
}
