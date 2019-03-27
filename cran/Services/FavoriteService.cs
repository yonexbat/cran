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

        public async Task<PagedResultDto<CourseDto>> GetFavoriteCourseAsync(int page)
        {
            string userId = this.GetUserId();
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
            return await ToDtoListAsync(list, ToCourseDto);
        }



        private async Task<CourseDto> ToCourseDto(Course course)
        {
            CourseDto courseVm = new CourseDto
            {
                Id = course.Id,
                Title = course.Title,
                Language = course.Language.ToString(),
                Description = course.Description,
                NumQuestionsToAsk = course.NumQuestionsToAsk,
                IsEditable = _currentPrincipal.IsInRole(Roles.Admin),
            };

            foreach (RelCourseTag relTag in course.RelTags)
            {
                Tag tag = relTag.Tag;
                TagDto tagVm = new TagDto
                {
                    Id = tag.Id,
                    IdTagType = (int)tag.TagType,
                    Description = tag.Description,
                    Name = tag.Name,
                    ShortDescDe = tag.ShortDescDe,
                    ShortDescEn = tag.ShortDescEn,
                };
                courseVm.Tags.Add(tagVm);
            }

            string userid = GetUserId();
            courseVm.IsFavorite = await _context.RelUserCourseFavorites.AnyAsync(x => x.Course.Id == course.Id && x.User.UserId == userid);

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
