using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Entities
{
    public class QuestionOption
    {
        public virtual int Id { get; set; }
        public virtual int IdQuestion { get; set; }
        public virtual bool IsTrue { get; set; }
        public virtual string Text { get; set; }

        public virtual string InsertUser { get; set; }
        public virtual DateTime InsertDate { get; set; }

        public virtual string UpdateUser { get; set; }
        public virtual DateTime UpdateDate { get; set; }

        public virtual Question Question { get; set; }
    }
}
