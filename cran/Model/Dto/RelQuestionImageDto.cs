using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Dto
{
    public class RelQuestionImageDto : IDto
    {
        public int Id { get; set; }
        public int IdQuestion { get; set; }
        public int IdImage { get; set; }
        public int IdBinary { get; set; }        
    }
}
