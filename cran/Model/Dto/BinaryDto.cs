using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Dto
{
    public class BinaryDto
    {
        public int Id { get; set; }
        public string ContentType { get; set; }
        public string ContentDisposition { get; set; }
        public string FileName { get; set; }
        public string Name { get; set; }
        public long Length { get; set; }
    }
}