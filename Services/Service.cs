using cran.Data;
using cran.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace cran.Services
{
    public abstract class Service
    {

        protected ApplicationDbContext _context;
        protected IPrincipal _currentPrincipal;

        public Service(ApplicationDbContext context, IPrincipal principal)
        {
            this._context = context;
            this._currentPrincipal = principal;
        }

        protected void InitTechnicalFields(ICraniumEntity entity)
        {
            if (entity.Id == 0)
            {
                entity.InsertDate = DateTime.Now;
                entity.InsertUser = GetUserId();
            }
            entity.UpdateDate = DateTime.Now;
            entity.UpdateUser = GetUserId();
        }

        public string GetUserId()
        {
            return _currentPrincipal.Identity.Name;
        }
    }
}
