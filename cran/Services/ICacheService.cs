using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Services
{
    public interface ICacheService
    {
        Task<T> GetEntryAsync<T>(string key, Func<Task<T>> func);

        void Clear();
    }
}
