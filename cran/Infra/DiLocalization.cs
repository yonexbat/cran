using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Infra
{
    public static class DiLocalization
    {
        public static void AddCranLocalization(this IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.Configure<RequestLocalizationOptions>(
                 opts =>
                 {
                     var supportedCultures = new List<CultureInfo>
                     {
                                    new CultureInfo("de-CH"),
                                    new CultureInfo("de"),
                                    new CultureInfo("en"),
                     };

                     opts.DefaultRequestCulture = new RequestCulture("de-CH");

                                // Formatting numbers, dates, etc.
                                opts.SupportedCultures = supportedCultures;
                                // UI strings that we have localized.
                                opts.SupportedUICultures = supportedCultures;
                 });
        }
    }
}
