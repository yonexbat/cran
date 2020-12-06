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

        protected void UpdateRelation<Tdto, Tentity>(IList<Tdto> dtos, IList<Tentity> entities, Action<Tdto, Tentity> copyData) 
            where Tdto: IDto 
            where Tentity : CranEntity, IIdentifiable, new()
        {
            IEnumerable<int> idsEntities = entities.Select(x => x.Id);
            IEnumerable<int> idsDtos = dtos.Select(x => x.Id);
            IEnumerable<IIdentifiable> entitiesToDelete = entities.Where(x => idsDtos.All(id => id != x.Id)).Cast<IIdentifiable>();
            IEnumerable<IIdentifiable> entitiesToUpdate = entities.Where(x => idsDtos.Any(id => id == x.Id)).Cast<IIdentifiable>();
            IEnumerable<IIdentifiable> dtosToAdd = dtos.Where(x => x.Id <= 0).Cast<IIdentifiable>();
            
            //Delete
            foreach(IIdentifiable entity in entitiesToDelete)
            {
                _dbContext.Remove(entity);
            }

            //Update
            foreach(Tentity entity in entitiesToUpdate)
            {
                Tdto dto = dtos.Single(x => x.Id == entity.Id);
                copyData(dto, entity);
            }
            
            //Add
            foreach(Tdto dto in dtosToAdd)
            {
                Tentity entity = new Tentity();
                copyData(dto, entity);
                _dbContext.Set<Tentity>().Add(entity);
            }
        }
               
    }
}
