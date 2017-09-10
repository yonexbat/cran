using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Dto
{
    public interface IPagedResult
    {
        int CurrentPage { get; set; }
        int Numpages { get; set; }
        int Pagesize { get; set; }
        int Count { get; set; }
    }
}
