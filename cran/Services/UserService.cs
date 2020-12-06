using cran.Data;
using cran.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace cran.Services
{
    public sealed class UserService : IUserService
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly ISecurityService _securityService;

        public UserService(ApplicationDbContext context, ISecurityService securityService)
        {
            this._dbContext = context;
            this._securityService = securityService;
        }


        public async Task<CranUser> GetOrCreateCranUserAsync()
        {
            string userId = _securityService.GetUserId();
            CranUser cranUserEntity = await _dbContext.CranUsers.Where(x => x.UserId == userId).SingleOrDefaultAsync();
            if (cranUserEntity == null)
            {
                cranUserEntity = new CranUser
                {
                    UserId = userId,
                    IsAnonymous = true,
                };
                await _dbContext.AddAsync(cranUserEntity);
            }
            await _dbContext.SaveChangesAsync();
            return cranUserEntity;
        }
     
    }
}
