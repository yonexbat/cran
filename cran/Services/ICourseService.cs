using cran.Model.Dto;
using cran.Model.Entities;
using System.Threading.Tasks;

namespace cran.Services
{
    public interface ICourseService
    {
        Task<PagedResultDto<CourseDto>> GetCoursesAsync(int page);
        Task<CourseDto> GetCourseAsync(int id);
        Task<int> InsertCourseAsync(CourseDto vm);
        Task UpdateCourseAsync(CourseDto vm);
    }
}
