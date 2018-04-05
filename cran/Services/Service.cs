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
        protected IPrincipal _currentPrincipal;

        public Service(ApplicationDbContext context, IPrincipal principal)
        {
            this._context = context;
            this._currentPrincipal = principal;
        }
       
        protected async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesCranAsync(_currentPrincipal);
        }

        public string GetUserId()
        {
            return _currentPrincipal.Identity.Name;
        }

        protected async Task<CranUser> GetCranUserAsync()
        {
            string userId = GetUserId();
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
