using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Services
{
    public interface IBusinessSecurityService
    {
        Task<bool> HasWriteAccess(int idUser);
    }
}
