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
        public static void ConfigureRoutes(IEndpointRouteBuilder rb)
        {
            rb.MapFallbackToController("Index", "Home");

            rb.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");            
                              
        }
    }
}
