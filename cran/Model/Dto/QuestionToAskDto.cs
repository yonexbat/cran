using cran.Model.Entities;
using System.Collections.Generic;

namespace cran.Model.Dto
{
    public class QuestionToAskDto
    {
        public int IdCourseInstanceQuestion { get; set; }
        public int IdCourseInstance { get; set; }
        public int IdQuestion { get; set; }
        public string Text { get; set; }
        public int NumQuestions { get; set; }
        public int NumCurrentQuestion { get; set; }
        public bool CourseEnded { get; set; }
        public bool Answered { get; set; }
        public bool AnswerShown { get; set; }
        public QuestionType QuestionType { get; set; }


        public IList<QuestionOptionToAskDto> Options { get; set; } = new List<QuestionOptionToAskDto>();
        public IList<ImageDto> Images { get; set; } = new List<ImageDto>();
        public IList<QuestionSelectorInfoDto> QuestionSelectors { get; set; } = new List<QuestionSelectorInfoDto>();
    }
}
