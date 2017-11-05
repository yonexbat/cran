using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Dto
{
    public class SearchQParametersDto
    {
        public int Page { get; set; }
        public string Title { get; set; }
        public string Language { get; set; }
        public int? Status { get; set; }
        public IList<TagDto> AndTags { get; set; } = new List<TagDto>();
        public IList<TagDto> OrTags { get; set; } = new List<TagDto>();
    }
}
