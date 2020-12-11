using cran.Data;
using cran.Model;
using cran.Model.Dto;
using cran.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Services.Util
{
    public static class ListUtil
    {
        public static IList<T> ToDtoList<T, Q>(IList<Q> input, Func<Q, T> func)
        {
            IList<T> result = new List<T>();
            foreach (Q q in input)
            {
                T t = func(q);
                result.Add(t);
            }
            return result;
        }

        public static async Task<IList<T>> ToDtoListAsync<T, Q>(IList<Q> input, Func<Q, Task<T>> func)
        {
            IList<T> result = new List<T>();
            foreach (Q q in input)
            {
                T t = await func(q);
                result.Add(t);
            }
            return result;
        }

        public static void UpdateRelation<Tdto, Tentity>(this ApplicationDbContext dbContext, IList<Tdto> dtos, IList<Tentity> entities, Action<Tdto, Tentity> copyData)
            where Tdto : IDto
            where Tentity : CranEntity, IIdentifiable, new()
        {
            IEnumerable<int> idsEntities = entities.Select(x => x.Id);
            IEnumerable<int> idsDtos = dtos.Select(x => x.Id);
            IEnumerable<IIdentifiable> entitiesToDelete = entities.Where(x => idsDtos.All(id => id != x.Id)).Cast<IIdentifiable>();
            IEnumerable<IIdentifiable> entitiesToUpdate = entities.Where(x => idsDtos.Any(id => id == x.Id)).Cast<IIdentifiable>();
            IEnumerable<IIdentifiable> dtosToAdd = dtos.Where(x => x.Id <= 0).Cast<IIdentifiable>();

            //Delete
            foreach (IIdentifiable entity in entitiesToDelete)
            {
                dbContext.Remove(entity);
            }

            //Update
            foreach (Tentity entity in entitiesToUpdate)
            {
                Tdto dto = dtos.Single(x => x.Id == entity.Id);
                copyData(dto, entity);
            }

            //Add
            foreach (Tdto dto in dtosToAdd)
            {
                Tentity entity = new Tentity();
                copyData(dto, entity);
                dbContext.Set<Tentity>().Add(entity);
            }
        }
    }
}
