using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Dto
{
    public class ImageDto
    {
        public int Id { get; set; }
        public int IdBinary { get; set; }
        public virtual int? Width { get; set; }
        public virtual int? Height { get; set; }
        public virtual bool Full { get; set; }
    }
}
