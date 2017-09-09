using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Dto
{
    public class CommentsDto
    {
        public int NumComments { get; set; }
        PagedResultDto<CommentDto> Comments { get; set; } = new PagedResultDto<CommentDto>();
    }
}
