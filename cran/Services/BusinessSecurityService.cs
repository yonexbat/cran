using cran.Data;
using cran.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Services
{
    public class BusinessSecurityService : IBusinessSecurityService
    {
        protected readonly IDbLogService _dbLogService;
        private readonly ISecurityService _securityService;
        private readonly ApplicationDbContext _dbContext;

        public BusinessSecurityService(ApplicationDbContext context, ISecurityService securityService)
        {
            _dbContext = context;
            _securityService = securityService;
        }

        public async Task<bool> HasWriteAccess(int idUser)
        {
            CranUser cranUser = await _dbContext.FindAsync<CranUser>(idUser);

            //Security Check
            if (cranUser.UserId == _securityService.GetUserId() || _securityService.IsInRole(Roles.Admin))
            {
                return true;
            }
            return false;
        }
    }
}
