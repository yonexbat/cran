using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Dto
{
    public class QuestionSelectorInfoDto
    {        
        public int IdCourseInstanceQuestion { get; set; }
        public int Number { get; set; }
        public bool? Correct { get; set; }
        public bool AnswerShown { get; set; }

    }
}
