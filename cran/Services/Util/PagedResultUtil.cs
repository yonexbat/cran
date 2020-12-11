using cran.Model.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Services.Util
{
    public static class PagedResultUtil
    {
        public static int PageSize = 5;

        public static  int InitPagedResult(IPagedResult pagedResult, int count, int page)
        {
            pagedResult.Count = count;
            pagedResult.Pagesize = PageSize;
            pagedResult.Numpages = CalculateNumPages(count);
            pagedResult.CurrentPage = pagedResult.Numpages >= page ? page : pagedResult.Numpages;
            return pagedResult.CurrentPage * PageSize;
        }

        public static async Task<PagedResultDto<TModel>> ToPagedResult<TEntity, TModel>(IQueryable<TEntity> queryBeforeSkipAndTake,
           int page, Func<IQueryable<TEntity>, Task<IList<TModel>>> toDtoFunc)
               where TModel : class
        {
            PagedResultDto<TModel> resultDto = new PagedResultDto<TModel>();

            //Count und paging.
            int count = await queryBeforeSkipAndTake.CountAsync();
            int startindex = InitPagedResult(resultDto, count, page);

            //Daten 
            IQueryable<TEntity> query = queryBeforeSkipAndTake.Skip(startindex).Take(PageSize);

            resultDto.Data = await toDtoFunc(query);

            return resultDto;
        }

        private static int CalculateNumPages(int count)
        {
            return ((count + PageSize - 1) / PageSize);
        }

    }
}
