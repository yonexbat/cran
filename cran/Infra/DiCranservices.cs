using cran.Model.Entities;
using cran.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace cran.Infra
{
    public static class DiCranservices
    {
        public static void AddCranServices(this IServiceCollection services)
        {
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
        }
    }
}
