using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Dto
{
    public class ResultDto
    {
        public int IdCourseInstance { get; set; }
        public IList<QuestionResultDto> Questions { get; set; } = new List<QuestionResultDto>();
    }
}
