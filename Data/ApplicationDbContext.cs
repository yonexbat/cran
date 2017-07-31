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
        public DbSet<LogEntry> LogEntires { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionOption> QuestionOptions { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            MapCourse(builder.Entity<Course>());
            MapLogEntry(builder.Entity<LogEntry>());
            MapQuestion(builder.Entity<Question>());
            MapQuestionOption(builder.Entity<QuestionOption>());
            
        }

        private void MapLogEntry(EntityTypeBuilder<LogEntry> typeBuilder)
        {
            typeBuilder.ToTable("CranLogEntry");
            typeBuilder.Property(x => x.Id).HasColumnName("Id");
            typeBuilder.HasKey(x => x.Id);
        }

        private void MapCourse(EntityTypeBuilder<Course> typeBuilder)
        {
            typeBuilder.ToTable("CranCourse");
            typeBuilder.Property(x => x.Id).HasColumnName("Id");           
            typeBuilder.HasKey(x => x.Id);
        }

        private void MapQuestionOption(EntityTypeBuilder<QuestionOption> typeBuilder)
        {
            typeBuilder.ToTable("CranQuestionOption");                        
            typeBuilder.HasOne(x => x.Question).WithMany(x => x.Options).HasForeignKey(o => o.IdQuestion);
        }

        private void MapQuestion(EntityTypeBuilder<Question> typeBuilder)
        {
            typeBuilder.ToTable("CranQuestion");
            typeBuilder
                .HasMany(x => x.Options)
                .WithOne(o => o.Question)
                .HasForeignKey(x => x.IdQuestion);

            /*
            typeBuilder.Property(x => x.Id).HasColumnName("Id");
            typeBuilder.HasKey(x => x.Id);
            typeBuilder.HasMany(x => x.Options).WithOne(x => x.Question).HasForeignKey("QuestionId");
            */
        }
    }
}
