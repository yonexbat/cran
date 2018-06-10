using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Entities
{
    public class CourseInstanceQuestionOption : CranEntity
    {
        public virtual int IdCourseInstanceQuestion { get; set; }
        public virtual int IdQuestionOption { get; set; }
        public virtual bool Correct { get; set; }
        public virtual bool Checked { get; set; }
        public virtual CourseInstanceQuestion CourseInstanceQuestion { get; set; }
        public virtual QuestionOption QuestionOption { get; set; }
    }
}
