using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Infra
{
    public static class DiGoogleAuth
    {
        public static void AddCranGoogleAuth(this IServiceCollection services, IConfigurationRoot configuration)
        {

            string clientId = configuration["CranSettings:ClientId"];
            string clientSecret = configuration["CranSettings:ClientSecret"];

            services.AddAuthentication()
                .AddGoogle(options => {
                    options.ClientId = clientId;
                    options.ClientSecret = clientSecret;
                });
        }
    }
}
