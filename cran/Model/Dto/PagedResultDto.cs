using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Dto
{
    public class PagedResultDto<T> : IPagedResult where T: class
    {
        public IList<T> Data { get; set; }
        public int CurrentPage { get; set; }
        public int Numpages { get; set; }
        public int Pagesize { get; set; }
        public int Count { get; set; }
    }
}
