using cran.Data;
using cran.Model.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Infra
{
    public static class DiApplicationDbContext
    {
        public static void AddCranDbContext(this IServiceCollection services, IConfigurationRoot configuration)
        {
            string connString = configuration["CranSettings:ConnectionString"];

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connString)
                .UseLazyLoadingProxies());

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        }
    }
}
