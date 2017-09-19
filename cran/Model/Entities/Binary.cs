using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Entities
{
    public class Binary : CranEntity
    {
        public virtual string ContentType { get; set; }
        public virtual string ContentDisposition { get; set; }
        public virtual string FileName { get; set; }
        public virtual string Name { get; set; }
        public virtual int Length { get;set;}

        public virtual IList<RelQuestionBinary> RelQuestions { get; set; } = new List<RelQuestionBinary>();
    }
}
