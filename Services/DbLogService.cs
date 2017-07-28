using cran.Data;
using cran.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Services
{
    public class DbLogService : IDbLogService
    {
        private ApplicationDbContext _context;

        public DbLogService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task LogMessageAsync(string message)
        {
            LogEntry logEntry = new LogEntry();
            logEntry.Created = DateTime.Now;
            logEntry.Message = message;
            _context.LogEntires.Add(logEntry);
            await _context.SaveChangesAsync();
        }
    }
}
