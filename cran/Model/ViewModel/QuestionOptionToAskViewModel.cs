using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.ViewModel
{
    public class QuestionOptionToAskViewModel
    {
        public bool IsTrue { get; set; }
        public bool IsChecked { get; set; }
        public int CourseInstanceQuestionOptionId { get; set; }
        public string Text { get; set; }
    }
}
