using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Entities
{
    public class Binary : CranEntity
    {
        public virtual int IdUser { get; set; }
        public virtual string ContentType { get; set; }
        public virtual string ContentDisposition { get; set; }
        public virtual string FileName { get; set; }
        public virtual string Name { get; set; }
        public virtual int Length { get; set; }

        public virtual CranUser User {get; set;}
        public virtual IList<Image> Images { get; set; } = new List<Image>();
    }
}
