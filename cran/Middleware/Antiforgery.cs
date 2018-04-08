using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Middleware
{
    public static class Antiforgery
    {
        public static void AddAntiforgery(this IApplicationBuilder builder, IAntiforgery antiforgery)
        {
            builder.Use(async (context, next) =>
            {
                await AddCookie(antiforgery, context, next);
            });
        }

        static async Task AddCookie(IAntiforgery antiforgery, HttpContext context, Func<Task> next)
        {
            if (context.Request.IsHttps)
            {
                string path = context.Request.Path.Value;
                if (path != null && !path.ToLower().Contains("/api"))
                {
                    // XSRF-TOKEN used by angular in the $http if provided
                    var tokens = antiforgery.GetAndStoreTokens(context);
                    context.Response.Cookies.Append("XSRF-TOKEN",
                      tokens.RequestToken, new CookieOptions
                      {
                          HttpOnly = false,
                          Secure = true
                      }
                    );
                }
            }
            await next();
        }
    }
}
