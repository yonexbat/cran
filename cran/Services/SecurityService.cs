using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using cran.Data;

namespace cran.Services
{
    public class SecurityService : Service
    {
        public SecurityService(ApplicationDbContext context, IPrincipal principal) : base(context, principal)
        {
            
        }
    }
}
