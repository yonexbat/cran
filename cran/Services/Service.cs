using cran.Data;
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
       
        protected async Task<int> SaveChanges()
        {
            return await _context.SaveChangesCranAsync(_currentPrincipal);
        }

        public string GetUserId()
        {
            return _currentPrincipal.Identity.Name;
        }
    }
}
