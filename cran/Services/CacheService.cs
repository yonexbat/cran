using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Services
{
    public class CacheService : ICacheService
    {

        private static IDictionary<string, object> _entries = new ConcurrentDictionary<string, object>();



        public async Task<T> GetEntryAsync<T>(string key, Func<Task<T>> func)
        {
            if(_entries.ContainsKey(key))
            {
                object o = _entries[key];
                T result = (T)o;
                return result;
            }

            T r = await func();
            _entries[key] = r;
            return r;
        }
    }
}
