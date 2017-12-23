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
        public virtual int IdUser { get; set; }
        public virtual int? IdQuestionSucessor { get; set; }
        public virtual QuestionStatus Status { get; set; }
        public virtual Language Language { get; set; }

        public virtual CranUser User { get; set; }
        public virtual IList<QuestionOption> Options { get; set; } = new List<QuestionOption>();
        public virtual IList<RelQuestionTag> RelTags { get; set; } = new List<RelQuestionTag>();
        public virtual IList<CourseInstanceQuestion> CourseInstancesQuestion { get; set; } = new List<CourseInstanceQuestion>();
        public virtual IList<Comment> Comments { get; set; } = new List<Comment>();
        public virtual IList<Rating> Ratings { get; set; } = new List<Rating>();
        public virtual IList<RelQuestionImage> RelImages { get; set; } = new List<RelQuestionImage>();
        public virtual Question Successor { get; set; }
        public virtual Question Predecessor { get; set; }
    }
}
