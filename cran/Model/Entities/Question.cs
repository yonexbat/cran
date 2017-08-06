using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Entities
{
    public class Question : CranEntity
    {

        public virtual string Title { get; set; }
        public virtual string Text { get; set; }
        public virtual string Explanation { get; set; }


        public virtual IList<QuestionOption> Options { get; set; } = new List<QuestionOption>();
        public virtual IList<RelQuestionTag> RelTags { get; set; } = new List<RelQuestionTag>();
        public virtual IList<CourseInstanceQuestion> CourseInstancesQuestion { get; set; } = new List<CourseInstanceQuestion>();
    }
}
