using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using cran.Data;
using cran.Model.Dto;
using cran.Model.Entities;
using cran.Services.Util;
using Microsoft.EntityFrameworkCore;

namespace cran.Services
{
    public class TextService : ITextService
    {
        private readonly ICultureService _cultureService;
        private readonly ApplicationDbContext _dbContext;

        public TextService(ApplicationDbContext context,  
            ICultureService cultureService)
        {
            _cultureService = cultureService;
            _dbContext = context;
        }

        public async Task<string> GetTextAsync(string key, params string[] placeholders)
        {            
            Text template = await _dbContext.Texts.Where(x => x.Key == key).SingleAsync();           
            string templateContent = template.ContentDe;
            switch(_cultureService.GetCurrentLanguage())
            {
                case Language.En:
                    templateContent = template.ContentEn;
                    break;
                case Language.De:
                    templateContent = template.ContentDe;
                    break;
            }           
            string result = string.Format(templateContent, placeholders);
            return result;
        }

        public async Task<TextDto> GetTextDtoAsync(int id)
        {
            Text text = await _dbContext.FindAsync<Text>(id);
            return ToDtoSingle(text);
        }

        public async Task<TextDto> GetTextDtoByKeyAsync(string key)
        {
            Text text = await _dbContext.Texts.Where(x => x.Key == key)
                .SingleOrDefaultAsync();
            return ToDtoSingle(text);
        }

        public async Task<PagedResultDto<TextDto>> GetTextsAsync(SearchTextDto parameters)
        {
            IQueryable<Text> query = _dbContext.Texts
                 .OrderBy(x => x.Key)
                 .ThenBy(x => x.Id);

            PagedResultDto<TextDto> result = await PagedResultUtil.ToPagedResult(query, parameters.Page, ToDto);
            return result;
        }
      

        public async Task UpdateTextAsync(TextDto vm)
        {
            Text text = await _dbContext.FindAsync<Text>(vm.Id);
            text.ContentDe = vm.ContentDe;
            text.ContentEn = vm.ContentEn;
            await _dbContext.SaveChangesAsync();
        }

        private async Task<IList<TextDto>> ToDto(IQueryable<Text> query)
        {
            return await query.Select(x => new TextDto
            {
                Id = x.Id,
                Key = x.Key,
                ContentDe = x.ContentDe,
                ContentEn = x.ContentEn,
            }).ToListAsync();
        }

        private TextDto ToDtoSingle(Text entity)
        {
            return new TextDto()
            {
                Id = entity.Id,
                Key = entity.Key,
                ContentDe = entity.ContentDe,
                ContentEn = entity.ContentEn,
            };
        }
    }
}
