using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Entities
{
    public class Rating : CranEntity
    {
        public virtual int IdUser { get; set; }
        public virtual int IdQuestion { get; set; }
        public virtual int QuestionRating { get; set; }

        public virtual CranUser User { get; set; }
        public virtual Question Question { get; set; }
    }
}
