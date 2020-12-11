using cran.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cran.Model.Entities;
using System.Security.Principal;
using cran.Model.Dto;
using cran.Model;
using Microsoft.EntityFrameworkCore;
using cran.Model.Dto.Notification;

namespace cran.Services
{
    public abstract class CraniumService
    {

        protected readonly IDbLogService _dbLogService;
        private readonly ISecurityService _securityService;
        private readonly ApplicationDbContext _dbContext;        


        public CraniumService(ApplicationDbContext context, IDbLogService dbLogService, ISecurityService securityService)
        {
            _dbContext = context;
            _dbLogService = dbLogService;
            _securityService = securityService;
        }               
    }
}
