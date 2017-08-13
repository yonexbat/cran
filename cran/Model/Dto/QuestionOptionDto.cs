using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Dto
{
    public class QuestionOptionDto
    {
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public bool IsTrue { get; set; }
    }
}
