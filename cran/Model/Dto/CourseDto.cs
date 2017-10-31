using System.Collections.Generic;


namespace cran.Model.Dto
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsEditable { get; set; }
        public int NumQuestionsToAsk { get; set; }
        public string Language { get; set; }

        public IList<TagDto> Tags = new List<TagDto>();        

    }
}
