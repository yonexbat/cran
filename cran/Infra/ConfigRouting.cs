using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Infra
{
    public static class ConfigRouting
    {
        public static void ConfigureRoutes(IRouteBuilder rb)
        {
            rb.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

            rb.MapSpaFallbackRoute(
                name: "spa-fallback",
                defaults: new { controller = "Home", action = "Index" });
        }
    }
}
