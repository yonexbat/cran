using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Dto
{
    public class QuestionToAskDto
    {
        public int IdCourseInstanceQuestion { get; set; }
        public string Text { get; set; }
        public IList<QuestionOptionToAskDto> Options = new List<QuestionOptionToAskDto>();
    }
}
