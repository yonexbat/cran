using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Dto
{
    public class QuestionOptionToAskDto
    {
        public bool IsTrue { get; set; }
        public bool IsChecked { get; set; }
        public int IdCourseInstanceQuestionOption { get; set; }
        public string Text { get; set; }
    }
}
