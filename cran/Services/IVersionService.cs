using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Services
{
    public interface IVersionService
    {
        Task AcceptQuestionAsync(int id);
        Task<int> VersionQuestionAsync(int id);
        Task<int> CopyQuestionAsync(int id);
    }
}
