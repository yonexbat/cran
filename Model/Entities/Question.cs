using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Entities
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }

        public string InsertUser { get; set; }
        public DateTime InsertDate { get; set; }

        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
