using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using cran.Model;
using cran.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using cran.Model.Entities;
using Microsoft.Extensions.FileProviders;
using cran.Services;
using System.Security.Principal;
using Microsoft.AspNetCore.Http;

namespace cran
{
    public class Startup
    {

        private IFileProvider _physicalProvider;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .AddUserSecrets<Startup>();

            _physicalProvider = env.ContentRootFileProvider;

            Configuration = builder.Build();          
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            
            string connString = Configuration["ConnectionString"];

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connString));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            string clientId = Configuration["ClientId"];
            string clientSecret = Configuration["ClientSecret"];          

            services.AddAuthentication()
                .AddGoogle(options => {
                    options.ClientId = clientId;
                    options.ClientSecret = clientSecret;
                });


            //Transient: for every object that required it a (new instance).
            //Scoped: once per request.
            //Singleton: self explained.

            services.AddTransient<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);

            services.AddScoped<SignInManager<ApplicationUser>, SignInManager<ApplicationUser>>();
            services.AddScoped<IDbLogService, DbLogService>();
            services.AddScoped<ICraniumService, CraniumService>();
            services.AddScoped<ISecurityService, SecurityService>();

            services.AddSingleton(_physicalProvider);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //Exception page
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();               
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            //Google login
            app.UseAuthentication();      

           

            //Routes
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
                
            });          
        }
    }
}
