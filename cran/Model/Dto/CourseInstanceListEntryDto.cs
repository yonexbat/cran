using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Dto
{
    public class CourseInstanceListEntryDto
    {
        public int IdCourseInstance { get; set; }
        public string Title { get; set; }
        public int NumQuestionsTotal { get; set; }
        public int NumQuestionsCorrect { get; set; }
        public int Percentage => NumQuestionsTotal > 0 ? 100 * NumQuestionsCorrect / NumQuestionsTotal : 0;
        public DateTime InsertDate { get; set; }
    }
}
