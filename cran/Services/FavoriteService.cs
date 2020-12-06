using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using cran.Data;
using cran.Mappers;
using cran.Model.Dto;
using cran.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace cran.Services
{
    public class FavoriteService : CraniumService, IFavoriteService
    {
        private ISecurityService _securityService;
        public FavoriteService(ApplicationDbContext context, IDbLogService dbLogService, ISecurityService securityService) 
            : base(context, dbLogService, securityService)
        {
            _securityService = securityService;
        }

        public async Task AddCourseToFavoritesAsync(CourseToFavoritesDto dto)
        {

            CranUser cranUser = await this.GetCranUserAsync();
            Course course = await _context.Courses.FindAsync(dto.CourseId);

            bool exists = await _context.RelUserCourseFavorites.AnyAsync(x => x.User.Id == cranUser.Id && x.Course.Id == course.Id);
            if(!exists)
            {
                RelUserCourseFavorite relUserCourseFavorite = new RelUserCourseFavorite()
                {
                    User = cranUser,
                    Course = course,
                };
                _context.RelUserCourseFavorites.Add(relUserCourseFavorite);
                await SaveChangesAsync();
            }
        }

        public async Task<PagedResultDto<CourseDto>> GetFavoriteCourseAsync(int page)
        {
            string userId = this._securityService.GetUserId();
            IQueryable<Course> query = from x in _context.Courses
                                       join relUser in _context.RelUserCourseFavorites on x.Id equals relUser.Course.Id
                                       where
                                           relUser.User.UserId == userId
                                       select x;


            query = query.OrderBy(x => x.Title).ThenBy(x => x.Id);
            return await ToPagedResult(query, page, ToDto);
        }

        private async Task<IList<CourseDto>> ToDto(IQueryable<Course> query)
        {
            query = query.Include(x => x.RelTags)
               .ThenInclude(x => x.Tag);

            IList<Course> list = await query
               .Include(x => x.RelTags)
               .ThenInclude(x => x.Tag)
               .ToListAsync();
            return ToDtoList(list, ToCourseDto);
        }



        private CourseDto ToCourseDto(Course course)
        {
            bool isEditable = _securityService.IsInRole(Roles.Admin);
            CourseDto courseVm = course.Map(isEditable, true);                    
            return courseVm;
        }

        public async Task RemoveCoureFromFavoritesAsync(CourseToFavoritesDto dto)
        {
            CranUser cranUser = await this.GetCranUserAsync();

            RelUserCourseFavorite rel = await _context.RelUserCourseFavorites.Where(x => x.User.Id == cranUser.Id && x.Course.Id == dto.CourseId)
                .FirstOrDefaultAsync();
            
            if(rel != null)
            {
                _context.Remove(rel);
                await this.SaveChangesAsync();
            }
        }
    }
}
