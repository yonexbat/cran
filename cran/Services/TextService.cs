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
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentUICulture;
            Text template = await _context.Texts.Where(x => x.Key == key).SingleAsync();           
            string templateContent = template.ContentDe;
            if(cultureInfo != null && cultureInfo.Name == "en-US")
            {
                templateContent = template.ContentEn;
            }
            string result = string.Format(templateContent, placeholders);
            return result;
        }
    }
}
