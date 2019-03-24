using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using cran.Data;
using cran.Model.Dto;
using cran.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace cran.Services
{
    public class FavoriteService : CraniumService, IFavoriteService
    {
        public FavoriteService(ApplicationDbContext context, IDbLogService dbLogService, IPrincipal principal) 
            : base(context, dbLogService, principal)
        {
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

        public Task<PagedResultDto<CourseDto>> GetFavoriteCourseAsync(int page)
        {
            throw new NotImplementedException();
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
