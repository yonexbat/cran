using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using cran.Data;
using cran.Model.Dto;
using cran.Model.Entities;

namespace cran.Services
{
    public class UserProfileService : CraniumService, IUserProfileService
    {
        public UserProfileService(ApplicationDbContext context, IDbLogService dbLogService, IPrincipal principal) : base(context, dbLogService, principal)
        {
        }

        public async Task<UserInfoDto> GetUserInfoAsync()
        {
            CranUser user = await GetCranUserAsync();
            return new UserInfoDto
            {
                Name = user.UserId,
            };
        }
    }
}
