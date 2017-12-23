using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Services
{
    public class CraniumException : Exception
    {
        public CraniumException(string message) : base(message) {
        }
    }
}
