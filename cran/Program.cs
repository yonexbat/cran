using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using cran.Middleware;
using System;
using cran.Infra;
using Microsoft.Extensions.Hosting;

namespace cran
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            IWebHostBuilder webHostBuilder = WebHost.CreateDefaultBuilder(args)
               .UseStartup<Startup>();

            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            
            bool isDevelopment = environment == Environments.Development;

            if (isDevelopment)
            {
                webHostBuilder.UseKestrel(options =>
                {
                    options.ConfigureEndpoints(5000);
                });
            }
            
            webHostBuilder.ConfigureLogging(ConfigLogging.ConfigureLogging);
            return webHostBuilder;
        }     

    }
}
