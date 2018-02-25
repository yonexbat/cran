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
using Microsoft.EntityFrameworkCore;

namespace cran.Services
{
    public class TextService : CraniumService, ITextService
    {
        public TextService(ApplicationDbContext context, IDbLogService dbLogService, IPrincipal principal) : base(context, dbLogService, principal)
        {
        }

        public async Task<string> GetTextAsync(string key, params string[] placeholders)
        {
            CultureInfo uiCultureInfo = Thread.CurrentThread.CurrentUICulture;           
            Text template = await _context.Texts.Where(x => x.Key == key).SingleAsync();           
            string templateContent = template.ContentDe;
            if(uiCultureInfo != null && uiCultureInfo.Name.EndsWith("en"))
            {
                templateContent = template.ContentEn;
            }
            string result = string.Format(templateContent, placeholders);
            return result;
        }

        public async Task<TextDto> GetTextDtoAsync(int id)
        {
            Text text = await _context.FindAsync<Text>(id);
            return ToDtoSingle(text);
        }

        public async Task<PagedResultDto<TextDto>> GetTextsAsync(SearchTextDto parameters)
        {
            IQueryable<Text> query = _context.Texts
                 .OrderBy(x => x.Key)
                 .ThenBy(x => x.Id);

            PagedResultDto<TextDto> result = await ToPagedResult(query, parameters.Page, ToDto);
            return result;
        }
      

        public async Task UpdateTextAsync(TextDto vm)
        {
            Text text = await _context.FindAsync<Text>(vm.Id);
            text.ContentDe = vm.ContentDe;
            text.ContentEn = vm.ContentEn;
            await SaveChangesAsync();
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
