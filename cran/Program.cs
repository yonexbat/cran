using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;
using cran.Middleware;
using System;

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

            
           var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
           var isDevelopment = environment == EnvironmentName.Development;


           if (isDevelopment)
           {
               webHostBuilder.UseKestrel(options =>
               {
                   options.ConfigureEndpoints(5000);
               });
           }

            return webHostBuilder;
           
        }


    }
}
