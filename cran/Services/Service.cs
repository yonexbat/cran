using cran.Data;
using cran.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace cran.Services
{
    public abstract class Service
    {

        private readonly ApplicationDbContext _context;
        private readonly ISecurityService _securityService;

        public Service(ApplicationDbContext context, ISecurityService securityService)
        {
            this._context = context;
            this._securityService = securityService;
        }


        protected async Task<CranUser> GetOrCreateCranUserAsync()
        {
            string userId = _securityService.GetUserId();
            CranUser cranUserEntity = await _context.CranUsers.Where(x => x.UserId == userId).SingleOrDefaultAsync();
            if (cranUserEntity == null)
            {
                cranUserEntity = new CranUser
                {
                    UserId = userId,
                    IsAnonymous = true,
                };
                await _context.AddAsync(cranUserEntity);
            }
            await _context.SaveChangesAsync();
            return cranUserEntity;
        }
     
    }
}
