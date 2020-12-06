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

        protected ApplicationDbContext _context;
        private ISecurityService _securityService;

        public Service(ApplicationDbContext context, ISecurityService securityService)
        {
            this._context = context;
            this._securityService = securityService;
        }
       
        protected async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }


        protected async Task<CranUser> GetCranUserAsync()
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
            await SaveChangesAsync();
            return cranUserEntity;
        }
     
    }
}
