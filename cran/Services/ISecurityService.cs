﻿using cran.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Services
{
    public interface ISecurityService
    {
        IList<string> GetRolesOfUser();

        string GetUserId();

        bool IsInRole(string roleName);
    }
}
