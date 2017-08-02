using cran.Data;
using cran.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace cran.Services
{
    public class DbLogService : Service, IDbLogService
    {

        public DbLogService(ApplicationDbContext context, IPrincipal principal) : base(context, principal)
        {
            _context = context;
        }

        public async Task LogMessageAsync(string message)
        {
            LogEntry logEntry = new LogEntry();
            logEntry.Created = DateTime.Now;
            logEntry.Message = message;
            InitTechnicalFields(logEntry);

            _context.LogEntires.Add(logEntry);
            await _context.SaveChangesAsync();
        }
    }
}
