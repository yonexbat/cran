using cran.Model.Entities;
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
        public Language? Language { get; set; }
        public bool StatusCreated { get; set; }
        public bool StatusReleased { get; set; }
        public bool StatusObsolete { get; set; }
        public IList<TagDto> AndTags { get; set; } = new List<TagDto>();
        public IList<TagDto> OrTags { get; set; } = new List<TagDto>();
    }
}
