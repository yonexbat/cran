using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Services
{
    public interface ITextService
    {
        Task<string> GetTextAsync(string key, params string[] placeholders);
    }
}
