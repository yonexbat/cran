using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using cran.Middleware;
using System;
using Microsoft.Extensions.Logging;

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
            bool isDevelopment = environment == EnvironmentName.Development;

            if (isDevelopment)
            {
                webHostBuilder.UseKestrel(options =>
                {
                    options.ConfigureEndpoints(5000);
                });
            }

            
            webHostBuilder.ConfigureLogging(ConfigureLogging);

            return webHostBuilder;

        }

        public static void ConfigureLogging(WebHostBuilderContext hostingContext, ILoggingBuilder logging)
        {
            logging.ClearProviders();
            logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
            logging.AddConsole();
            logging.AddDebug();
            logging.AddEventSourceLogger();
        }



    }
}
