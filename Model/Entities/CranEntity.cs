using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Entities
{
    public class CranEntity : ICraniumEntity
    {
        public virtual int Id { get; set; }

        public virtual string InsertUser { get; set; }
        public virtual DateTime InsertDate { get; set; }

        public virtual string UpdateUser { get; set; }
        public virtual DateTime UpdateDate { get; set; }
    }
}
