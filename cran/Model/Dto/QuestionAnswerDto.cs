using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Dto
{
    public class QuestionAnswerDto
    {
        public int IdCourseInstanceQuestion;
        public IList<bool> Answers { get; set; } = new List<bool>();
    }
}
