using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using cran.Data;
using cran.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace cran.Services
{
    public class SecurityService : ISecurityService
    {
        protected IPrincipal _currentPrincipal;

        public SecurityService(IPrincipal principal)
        {
            this._currentPrincipal = principal;
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


        public string GetUserId()
        {
            return _currentPrincipal.Identity.Name;
        }

        public bool IsInRole(string roleName)
        {
            return _currentPrincipal.IsInRole(roleName);
        }
    }
}
