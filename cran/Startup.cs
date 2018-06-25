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
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using cran.Middleware;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Rewrite;
using System.Threading;
using Microsoft.AspNetCore.Mvc;

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
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc()               
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.Configure<RequestLocalizationOptions>(
                opts =>
                {
                    var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("de-CH"),
                        new CultureInfo("de"),
                        new CultureInfo("en"),
                    };

                    opts.DefaultRequestCulture = new RequestCulture("de-CH");
                    // Formatting numbers, dates, etc.
                    opts.SupportedCultures = supportedCultures;
                    // UI strings that we have localized.
                    opts.SupportedUICultures = supportedCultures;
                });


            string connString = Configuration["ConnectionString"];

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connString)
                .UseLazyLoadingProxies());

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

            services.AddAntiforgery(options => {
                options.HeaderName = "X-XSRF-TOKEN";               
            });


            //Transient: for every object that required it a (new instance).
            //Scoped: once per request.
            //Singleton: self explained.

            services.AddTransient<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);
            services.AddTransient<ICultureService, CultureService>();

            services.AddScoped<SignInManager<ApplicationUser>, SignInManager<ApplicationUser>>();
            services.AddScoped<IDbLogService, DbLogService>();
            services.AddScoped<ISecurityService, SecurityService>();
            services.AddScoped<IBinaryService, BinaryService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IVersionService, VersionService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ICommentsService, CommentsService>();
            services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddScoped<ICourseInstanceService, CourseInstanceService>();
            services.AddScoped<ITextService, TextService>();
            services.AddScoped<IExportService, ExportService>();
            services.AddSingleton<ICacheService, CacheService>();
            services.AddSingleton(_physicalProvider);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env, 
            ILoggerFactory loggerFactory,
            IAntiforgery antiforgery)
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

                //The HTTP Strict-Transport-Security response header (often abbreviated as HSTS)  
                //lets a web site tell browsers that it should only be accessed using HTTPS, instead of using HTTP.
                app.UseHsts();
            }

            //Redirect to             
            app.UseHttpsRedirection();

            //Static files
            app.UseStaticFiles();

            //Localization          
            IOptions<RequestLocalizationOptions> options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

            //Google login
            app.UseAuthentication();

            //AntiforgeryCookies
            app.AddAntiforgery(antiforgery);

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
