using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Dto
{
    public class QuestionToAskDto
    {
        public int IdCourseInstanceQuestion { get; set; }
        public int IdCourseInstance { get; set; }
        public int IdQuestion { get; set; }
        public string Text { get; set; }
        public int NumQuestions { get; set; }
        public int NumQuestionsAsked { get; set; }
        public bool CourseEnded { get; set; }
        public bool Answered { get; set; }
                
        public IList<QuestionOptionToAskDto> Options = new List<QuestionOptionToAskDto>();
    }
}
