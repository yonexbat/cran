using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using cran.Data;

namespace cran.Services
{
    public class FavoriteService : CraniumService, IFavoriteService
    {
        public FavoriteService(ApplicationDbContext context, IDbLogService dbLogService, IPrincipal principal) 
            : base(context, dbLogService, principal)
        {
        }
        

    }
}
