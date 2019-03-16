using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace cran.Infra
{
    public static class DiAccessDenied
    {
        public static void AddRedirectForForbidden(this IServiceCollection services)
        {
            services.ConfigureApplicationCookie(options => {
                options.Events.OnRedirectToAccessDenied = ReplaceRedirector(HttpStatusCode.Forbidden, options.Events.OnRedirectToAccessDenied);
                options.Events.OnRedirectToLogin = ReplaceRedirector(HttpStatusCode.Unauthorized, options.Events.OnRedirectToLogin);
            });
        }
        

        static Func<RedirectContext<CookieAuthenticationOptions>, Task> 
                ReplaceRedirector(
                    HttpStatusCode statusCode, 
                    Func<RedirectContext<CookieAuthenticationOptions>, Task> existingRedirector) =>
                            context => 
                            {
                                if (context.Request.Path.StartsWithSegments("/api"))
                                {
                                    context.Response.StatusCode = (int)statusCode;
                                    return Task.CompletedTask;
                                }
                                return existingRedirector(context);
                            };

    }
}
