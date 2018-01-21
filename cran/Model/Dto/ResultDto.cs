using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Dto
{
    public class ResultDto
    {
        public int IdCourse { get; set; }
        public int IdCourseInstance { get; set; }
        public string CourseTitle { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public IList<QuestionResultDto> Questions { get; set; } = new List<QuestionResultDto>();
    }
}
