using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Dto
{
    public class QuestionListEntryDto
    {
        public string Title { get; set; }
        public int Id { get; set; }
        public IList<TagDto> Tags { get; set; } = new List<TagDto>();
    }
}
