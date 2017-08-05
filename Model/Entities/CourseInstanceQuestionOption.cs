using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Entities
{
    public class CourseInstanceQuestionOption : CranEntity
    {
        public int IdCourseInstanceQuestion { get; set; }
        public int IdQuestionOption { get; set; }
        public bool Correct { get; set; }
        public CourseInstanceQuestion CourseInstanceQuestion { get; set; }
        public QuestionOption QuestionOption { get; set; }
    }
}
