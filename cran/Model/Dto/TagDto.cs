using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Dto
{
    public class TagDto : IIdentifiable
    {
        public int Id { get; set; }
        public int IdTagType { get; set; }
        public string Name { get; set; }
        public string ShortDescDe { get; set; }
        public string ShortDescEn { get; set; }
        public string Description { get; set; }
    }
}
