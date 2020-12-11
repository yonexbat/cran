using cran.Data;
using cran.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace cran.Services
{
    public class DbLogService : IDbLogService
    {
        private readonly ApplicationDbContext _dbContext;

        public DbLogService(ApplicationDbContext context) 
        {
            _dbContext = context;
        }

        public async Task LogMessageAsync(string message)
        {
            LogEntry logEntry = new LogEntry();
            logEntry.Created = DateTime.Now;
            logEntry.Message = message;

            _dbContext.LogEntires.Add(logEntry);
            await _dbContext.SaveChangesAsync();
        }
    }
}
