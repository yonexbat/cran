using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Entities
{
    public class QuestionOption : CranEntity
    {
        public virtual int IdQuestion { get; set; }
        public virtual bool IsTrue { get; set; }
        public virtual string Text { get; set; }      

        public virtual Question Question { get; set; }

        public virtual IList<CourseInstanceQuestionOption> CourseInstancesQuestionOption { get; set; } = new List<CourseInstanceQuestionOption>();
    }
}
