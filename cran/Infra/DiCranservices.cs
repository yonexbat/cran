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

            services.AddTransient<IPrincipal>((IServiceProvider provider) => GetPrincipal(provider));
            services.AddTransient<ICultureService, CultureService>();
            services.AddScoped<SignInManager<ApplicationUser>, SignInManager<ApplicationUser>>();
            services.AddScoped<IDbLogService, DbLogService>();
            services.AddScoped<ISecurityService, SecurityService>();
            services.AddScoped<IBinaryService, BinaryService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IBusinessSecurityService, BusinessSecurityService>();
            services.AddScoped<IVersionService, VersionService>();
            services.AddScoped<ITagService, TagService>();
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<ICommentsService, CommentsService>();
            services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddScoped<ICourseInstanceService, CourseInstanceService>();
            services.AddScoped<ITextService, TextService>();
            services.AddScoped<IExportService, ExportService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IFavoriteService, FavoriteService>();
            services.AddSingleton<ICacheService, CacheService>(); 
            services.AddSingleton<IWebPushClient, WebPushClient>();              
            
        }

        public static IPrincipal GetPrincipal(IServiceProvider provider)
        {
            IPrincipal principal = provider.GetService<IHttpContextAccessor>()?.HttpContext?.User;
            if(principal == null)
            {
                principal = new GenericPrincipal(new GenericIdentity("anonymous"), new string[0]);
            }

            return principal;
        }



    }
}
