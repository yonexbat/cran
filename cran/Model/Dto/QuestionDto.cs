using cran.Model.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace cran.Model.Dto
{
    public class QuestionDto : IDto
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Text { get; set; } = string.Empty;

        public string Explanation { get; set; }

        public int Status { get; set; }

        public bool IsEditable { get; set; }

        public IList<QuestionOptionDto> Options = new List<QuestionOptionDto>();

        public IList<TagDto> Tags = new List<TagDto>();

        public VotesDto Votes { get; set; }

        public IList<ImageDto> Images = new List<ImageDto>();
        
        public string Language { get;set; }

        public QuestionType QuestionType { get; set; }
    }
}
