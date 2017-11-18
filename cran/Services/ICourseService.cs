using cran.Model.Dto;
using System.Threading.Tasks;

namespace cran.Services
{
    public interface ICourseService
    {
        Task<CoursesDto> GetCoursesAsync();
        Task<CourseDto> GetCourseAsync(int id);
        Task<int> InsertCourseAsync(CourseDto vm);
        Task UpdateCourseAsync(CourseDto vm);
    }
}
