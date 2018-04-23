using cran.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Dto
{
    public class VersionInfoDto
    {
        public int IdQuestion { get; set; }
        public int Version { get; set; }
        public string User { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public QuestionStatus Status { get; set; }
    }
}
