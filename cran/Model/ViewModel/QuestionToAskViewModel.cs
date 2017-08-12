using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.ViewModel
{
    public class QuestionToAskViewModel
    {
        public int CourseInstanceQuestionId { get; set; }
        public string Text { get; set; }
        public IList<QuestionOptionToAskViewModel> Options = new List<QuestionOptionToAskViewModel>();
    }
}
