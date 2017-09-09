using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Dto
{
    public class CommentDto
    {
        public int IdComment { get; set; }
        public int IdQuestion { get; set; }
        public string CommentText { get; set; }
        public bool IsEditable { get; set; }
    }
}
