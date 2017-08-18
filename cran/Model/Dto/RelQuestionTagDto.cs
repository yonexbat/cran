using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Dto
{
    public class RelQuestionTagDto : IDto
    {
        public int Id { get; set; }
        public int IdTag { get; set; }
        public int IdQuestion { get; set; }
    }
}
