using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;
using cran.Middleware;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Mvc;
using cran.Infra;
using Microsoft.Extensions.Logging;
using Serilog;
using cran.Model.Dto;
using cran.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;

namespace cran
{
    public class Startup
    {

        private IFileProvider _physicalFileProvider;

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .AddUserSecrets<Startup>();

            _physicalFileProvider = env.ContentRootFileProvider;

            Configuration = builder.Build();          
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_physicalFileProvider);

            services.AddControllersWithViews()                
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization()                
                .SetCompatibilityVersion(CompatibilityVersion.Latest);
           

            services.AddMvc(options => options
                   .Filters.Add(typeof(AuditFilter)));



            services.AddCranLocalization();
            services.AddCranDbContext(Configuration);
            services.AddCranGoogleAuth(Configuration);
            services.AddCranServices();
            services.Configure<CranSettingsDto>(Configuration.GetSection("CranSettings"));
            services.AddRedirectForForbidden();

            services.AddAntiforgery(options => {
                options.HeaderName = "X-XSRF-TOKEN";               
            });
            
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env, 
            IAntiforgery antiforgery,
            ILoggerFactory loggerFactory)
        {            
            //Exception page
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                loggerFactory.AddFile("Logs/serilog-{Date}.txt");
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                //The HTTP Strict-Transport-Security response header (often abbreviated as HSTS)  
                //lets a web site tell browsers that it should only be accessed using HTTPS, instead of using HTTP.
                app.UseHsts();
            }

            app.UseRouting();

            //Redirect to  HTTPS
            app.UseRewriter(new RewriteOptions().AddRedirectToHttps());

            //Add Seri Log
            loggerFactory.AddSerilog();

            //Static files
            app.UseDefaultFiles();
            app.UseStaticFiles();
           

            //Localization          
            IOptions<RequestLocalizationOptions> options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

            //Security
            app.UseAuthentication();
            app.UseAuthorization();           
            //AntiforgeryCookies
            app.AddAntiforgery(antiforgery);

            app.UseEndpoints(endpoints =>
            {
                ConfigRouting.ConfigureRoutes(endpoints);
            });

        }
    }
}
