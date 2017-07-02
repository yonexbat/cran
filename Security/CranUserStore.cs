using cran.Model;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace cran.Security
{
    public class CranUserStore : IUserStore<CranUser>, IUserLoginStore<CranUser>
    {
        public async Task AddLoginAsync(CranUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
          
        }

        public async Task<IdentityResult> CreateAsync(CranUser user, CancellationToken cancellationToken)
        {
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(CranUser user, CancellationToken cancellationToken)
        {
            return IdentityResult.Success;
        }

        public void Dispose()
        {
            
        }

        public async Task<CranUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return new CranUser();
        }

        public async Task<CranUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            return new CranUser();
        }

        public async Task<CranUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return new CranUser();
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(CranUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetNormalizedUserNameAsync(CranUser user, CancellationToken cancellationToken)
        {
            return "hugo";
        }

        public async Task<string> GetUserIdAsync(CranUser user, CancellationToken cancellationToken)
        {
            return "hugo";
        }

        public async Task<string> GetUserNameAsync(CranUser user, CancellationToken cancellationToken)
        {
            return "hugo";
        }

        public async Task RemoveLoginAsync(CranUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        { 
        }

        public async Task SetNormalizedUserNameAsync(CranUser user, string normalizedName, CancellationToken cancellationToken)
        {
          
        }

        public async Task SetUserNameAsync(CranUser user, string userName, CancellationToken cancellationToken)
        {
            
        }

        public async Task<IdentityResult> UpdateAsync(CranUser user, CancellationToken cancellationToken)
        {
            return IdentityResult.Success;
        }
    }
}
