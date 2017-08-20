using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Entities
{
    public class Comment : CranEntity
    {
        public virtual int IdUser { get; set; }
        public virtual int IdQuestion { get; set; }
        public virtual string CommentText { get; set; }

        public virtual CranUser User { get; set; }
        public virtual Question Question { get; set; }
    }
}
