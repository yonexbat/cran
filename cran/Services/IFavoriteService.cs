using cran.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Services
{
    public interface IFavoriteService
    {
        Task<PagedResultDto<CourseDto>> GetFavoriteCourseAsync(int page);
        Task AddCourseToFavoritesAsync(CourseToFavoritesDto dto);
        Task RemoveCoureFromFavoritesAsync(CourseToFavoritesDto dto);
    }
}
