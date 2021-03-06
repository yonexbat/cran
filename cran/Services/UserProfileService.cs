﻿using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using cran.Data;
using cran.Model.Dto;
using cran.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace cran.Services
{
    public class UserProfileService : IUserProfileService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserService _userService;

        public UserProfileService(
            ApplicationDbContext context, 
            IUserService userService)
        {
            _dbContext = context;
            _userService = userService;
        }

        public async Task CreateUserAsync(UserInfoDto info)
        {
            CranUser user = await _dbContext.CranUsers.Where(x => x.UserId == info.Name).SingleOrDefaultAsync();
            if(user == null)
            {
                user = new CranUser
                {
                    UserId = info.Name,
                    IsAnonymous = info.IsAnonymous,
                };
                _dbContext.CranUsers.Add(user);
                await _dbContext.SaveChangesAsync();
            }         
        }

        public async Task<UserInfoDto> GetUserInfoAsync()
        {
            CranUser user = await _userService.GetOrCreateCranUserAsync();
            return ToProfileDto(user);
        }

        private UserInfoDto ToProfileDto(CranUser user)
        {
            return new UserInfoDto
            {
                Name = user.UserId,
                IsAnonymous = user.IsAnonymous,
            };
        }
    }
}
