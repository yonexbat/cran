using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using cran.Model.Entities;

namespace cran.Services
{
    public class CultureService : ICultureService
    {
        public Language GetCurrentLanguage()
        {
            CultureInfo uiCultureInfo = Thread.CurrentThread.CurrentUICulture;
            if (uiCultureInfo != null && uiCultureInfo.Name.EndsWith("en"))
            {
                return Language.En;
            }
            return Language.De;
        }
    }
}
