using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Entities
{
    public class Container : CranEntity
    {
        public virtual IList<Question> Questions { get; set; } = new List<Question>();
    }
}
