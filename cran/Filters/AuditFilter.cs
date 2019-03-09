using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Security.Principal;

namespace cran.Filters
{
    public class AuditFilter : IAuthorizationFilter
    {
        private ILogger _logger;

        public AuditFilter(ILogger<AuditFilter> logger)
        {
            _logger = logger;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {            
            string path = context.HttpContext.Request.Path;
            _logger.LogDebug(path);            
        }
    }
}
