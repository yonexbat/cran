using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Dto
{
    public class QuestionOptionDto : IDto
    {
        public int Id { get; set; }

        public int IdQuestion { get; set; }

        [Required]
        public string Text { get; set; }

        public bool IsTrue { get; set; }
    }
}
