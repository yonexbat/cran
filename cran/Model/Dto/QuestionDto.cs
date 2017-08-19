using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace cran.Model.Dto
{
    public class QuestionDto : IIdentifiable
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Text { get; set; }

        public string Explanation { get; set; }

        public int Status { get; set; }

        public IList<QuestionOptionDto> Options = new List<QuestionOptionDto>();

        public IList<TagDto> Tags = new List<TagDto>();
    }
}
