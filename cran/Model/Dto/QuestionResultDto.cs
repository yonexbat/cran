using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Dto
{
    public class QuestionResultDto
    {
        public string Title { get; set; }
        public bool Correct { get; set; }
        public int IdCourseInstanceQuestion { get; set; }
        
    }
}
