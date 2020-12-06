using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using cran.Data;
using cran.Mappers;
using cran.Model.Dto;
using cran.Model.Entities;
using cran.Services.Util;
using Microsoft.EntityFrameworkCore;

namespace cran.Services
{
    public class FavoriteService : CraniumService, IFavoriteService
    {
        private readonly ISecurityService _securityService;
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserService _userService;


        public FavoriteService(ApplicationDbContext context, IDbLogService dbLogService, ISecurityService securityService, IUserService userService) 
            : base(context, dbLogService, securityService)
        {
            _securityService = securityService;
            _dbContext = context;
            _userService = userService;
        }

        public async Task AddCourseToFavoritesAsync(CourseToFavoritesDto dto)
        {

            CranUser cranUser = await _userService.GetOrCreateCranUserAsync();
            Course course = await _dbContext.Courses.FindAsync(dto.CourseId);

            bool exists = await _dbContext.RelUserCourseFavorites.AnyAsync(x => x.User.Id == cranUser.Id && x.Course.Id == course.Id);
            if(!exists)
            {
                RelUserCourseFavorite relUserCourseFavorite = new RelUserCourseFavorite()
                {
                    User = cranUser,
                    Course = course,
                };
                _dbContext.RelUserCourseFavorites.Add(relUserCourseFavorite);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<PagedResultDto<CourseDto>> GetFavoriteCourseAsync(int page)
        {
            string userId = this._securityService.GetUserId();
            IQueryable<Course> query = from x in _dbContext.Courses
                                       join relUser in _dbContext.RelUserCourseFavorites on x.Id equals relUser.Course.Id
                                       where
                                           relUser.User.UserId == userId
                                       select x;


            query = query.OrderBy(x => x.Title).ThenBy(x => x.Id);
            return await PagedResultUtil.ToPagedResult(query, page, ToDto);
        }

        private async Task<IList<CourseDto>> ToDto(IQueryable<Course> query)
        {
            query = query.Include(x => x.RelTags)
               .ThenInclude(x => x.Tag);

            IList<Course> list = await query
               .Include(x => x.RelTags)
               .ThenInclude(x => x.Tag)
               .ToListAsync();
            return ListUtil.ToDtoList(list, ToCourseDto);
        }



        private CourseDto ToCourseDto(Course course)
        {
            bool isEditable = _securityService.IsInRole(Roles.Admin);
            CourseDto courseVm = course.Map(isEditable, true);                    
            return courseVm;
        }

        public async Task RemoveCoureFromFavoritesAsync(CourseToFavoritesDto dto)
        {
            CranUser cranUser = await _userService.GetOrCreateCranUserAsync();

            RelUserCourseFavorite rel = await _dbContext.RelUserCourseFavorites.Where(x => x.User.Id == cranUser.Id && x.Course.Id == dto.CourseId)
                .FirstOrDefaultAsync();
            
            if(rel != null)
            {
                _dbContext.Remove(rel);
                await this._dbContext.SaveChangesAsync();
            }
        }
    }
}
