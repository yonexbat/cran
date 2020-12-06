using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Services.Util
{
    public static class ListUtil
    {
        public static IList<T> ToDtoList<T, Q>(IList<Q> input, Func<Q, T> func)
        {
            IList<T> result = new List<T>();
            foreach (Q q in input)
            {
                T t = func(q);
                result.Add(t);
            }
            return result;
        }

        public static async Task<IList<T>> ToDtoListAsync<T, Q>(IList<Q> input, Func<Q, Task<T>> func)
        {
            IList<T> result = new List<T>();
            foreach (Q q in input)
            {
                T t = await func(q);
                result.Add(t);
            }
            return result;
        }
    }
}
