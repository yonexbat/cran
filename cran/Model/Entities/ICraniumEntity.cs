﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Model.Entities
{
    public interface ICraniumEntity : IIdentifiable
    {

        string InsertUser { get; set; }
        DateTime InsertDate { get; set; }

        string UpdateUser { get; set; }
        DateTime UpdateDate { get; set; }
    }
}
