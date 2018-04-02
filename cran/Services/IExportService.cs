using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Services
{
    public interface IExportService
    {
        Task<Stream> Export();
    }
}
