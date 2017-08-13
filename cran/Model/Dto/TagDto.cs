using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Dto
{
    public class TagDto
    {
        public int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
    }
}
