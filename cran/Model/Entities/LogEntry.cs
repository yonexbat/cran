using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Entities
{
    public class LogEntry : CranEntity
    {
        public string Message { get; set; }
        public DateTime Created { get; set; }
    }
}
