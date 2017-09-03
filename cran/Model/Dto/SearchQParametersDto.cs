using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Dto
{
    public class SearchQParametersDto
    {
        public int Page { get; set; }
        public IList<int> AndTags { get; set; } = new List<int>();
        public IList<int> OrTags { get; set; } = new List<int>();
    }
}
