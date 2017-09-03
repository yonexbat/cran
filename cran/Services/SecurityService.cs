using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using cran.Data;

namespace cran.Services
{
    public class SecurityService : Service, ISecurityService
    {
        public SecurityService(ApplicationDbContext context, IPrincipal principal) : base(context, principal)
        {
            
        }

        public IList<string> GetRolesOfUser()
        {
            IList<string> result = new List<string>();
            if(this._currentPrincipal.IsInRole(Roles.Admin))
            {
                result.Add(Roles.Admin);
            }
            if(this._currentPrincipal.IsInRole(Roles.User))
            {
                result.Add(Roles.User);
            }
            return result;
        }
    }
}
