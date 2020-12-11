using cran.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Services
{
    public interface IUserService
    {
        Task<CranUser> GetOrCreateCranUserAsync();
    }
}
