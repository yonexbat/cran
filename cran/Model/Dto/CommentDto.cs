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
        public string UserId { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public bool IsEditable { get; set; }
    }
}
