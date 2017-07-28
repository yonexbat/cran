using cran.Model.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Course> Courses { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            MapCourse(builder.Entity<Course>());
            
        }

        private void MapCourse(EntityTypeBuilder<Course> typeBuilder)
        {
            typeBuilder.ToTable("CranCourse");
            typeBuilder.Property(x => x.Id).HasColumnName("Id");           
            typeBuilder.HasKey(x => x.Id);
        }
    }
}
