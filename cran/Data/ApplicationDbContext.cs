using cran.Model.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Principal;

namespace cran.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<LogEntry> LogEntires { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionOption> QuestionOptions { get; set; }
        public DbSet<RelQuestionTag> RelQuestionTags { get; set; }
        public DbSet<RelCourseTag> RelCourseTags { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<CranUser> CranUsers { get; set; }
        public DbSet<CourseInstance> CourseInstances { get; set; }
        public DbSet<CourseInstanceQuestion> CourseInstancesQuestion { get; set; }
        public DbSet<CourseInstanceQuestionOption> CourseInstancesQuestionOption { get; set; }

        protected IPrincipal _principal;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IPrincipal principal)
            : base(options)
        {
            this._principal = principal;
        }

        public async Task<int> SaveChangesCranAsync(IPrincipal principal)
        {
            _principal = principal;
            return await SaveChangesAsync();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            InitTechnicalFields();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            InitTechnicalFields();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected void InitTechnicalFields()
        {
            ChangeTracker.DetectChanges();
            DateTime now = DateTime.Now;
            string user = GetUserId();

            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Added))
            {
                if (entry.Entity is CranEntity)
                {

                    CranEntity cranEntity = (CranEntity)entry.Entity;
                    cranEntity.InsertDate = now;
                    cranEntity.UpdateDate = now;
                    cranEntity.InsertUser = user;
                    cranEntity.UpdateUser = user;
                }
            }
            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Modified))
            {
                if (entry.Entity is CranEntity)
                {

                    CranEntity cranEntity = (CranEntity)entry.Entity;
                    cranEntity.UpdateDate = now;
                    cranEntity.UpdateUser = user;
                }
            }
        }

        protected string GetUserId()
        {
            return _principal.Identity.Name ?? string.Empty;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            MapCourse(builder.Entity<Course>());
            MapLogEntry(builder.Entity<LogEntry>());
            MapQuestion(builder.Entity<Question>());
            MapQuestionOption(builder.Entity<QuestionOption>());
            MapTag(builder.Entity<Tag>());
            MapRelCourseTag(builder.Entity<RelCourseTag>());
            MapRelQuestionTag(builder.Entity<RelQuestionTag>());
            MapCranUser(builder.Entity<CranUser>());
            MapCourseInstance(builder.Entity<CourseInstance>());
            MapCourseInstanceQuestion(builder.Entity<CourseInstanceQuestion>());
            MapCourseInstanceQuestionOption(builder.Entity<CourseInstanceQuestionOption>());
        }    
        
        private void MapCranUser(EntityTypeBuilder<CranUser> typeBuilder)
        {
            typeBuilder.ToTable("CranUser");
        }

        private void MapCourseInstance(EntityTypeBuilder<CourseInstance> typeBuilder)
        {
            typeBuilder.ToTable("CranCourseInstance");

            typeBuilder.HasOne(x => x.Course)
                .WithMany(c => c.CourseInstances)
                .HasForeignKey(x => x.IdCourse);

            typeBuilder.HasOne(x => x.User)
                .WithMany(u => u.CourseInstances)
                .HasForeignKey(x => x.IdUser);
        }

        private void MapCourseInstanceQuestion(EntityTypeBuilder<CourseInstanceQuestion> typeBuilder)
        {
            typeBuilder.ToTable("CranCourseInstanceQuestion");

            typeBuilder.HasOne(x => x.CourseInstance)
                .WithMany(i => i.CourseInstancesQuestion)
                .HasForeignKey(x => x.IdCourseInstance);

            typeBuilder.HasOne(x => x.Question)
                .WithMany(q => q.CourseInstancesQuestion).HasForeignKey(x => x.IdQuestion);
        }

        private void MapCourseInstanceQuestionOption(EntityTypeBuilder<CourseInstanceQuestionOption> typeBuilder)
        {
            typeBuilder.ToTable("CranCourseInstanceQuestionOption");

            typeBuilder.HasOne(x => x.QuestionOption)
                .WithMany(o => o.CourseInstancesQuestionOption)
                .HasForeignKey(x => x.IdQuestionOption);

            typeBuilder.HasOne(x => x.CourseInstanceQuestion)
                .WithMany(io => io.CourseInstancesQuestionOption)
                .HasForeignKey(x => x.IdCourseInstanceQuestion);
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
            typeBuilder
                .HasOne(x => x.Question)
                .WithMany(x => x.Options)
                .HasForeignKey(o => o.IdQuestion);
        }

        private void MapQuestion(EntityTypeBuilder<Question> typeBuilder)
        {
            typeBuilder.ToTable("CranQuestion");

            typeBuilder
                .HasMany(x => x.Options)
                .WithOne(o => o.Question)
                .HasForeignKey(x => x.IdQuestion);            
        }

        private void MapTag(EntityTypeBuilder<Tag> typeBuilder)
        {
            typeBuilder.ToTable("CranTag");            
        }

        private void MapRelCourseTag(EntityTypeBuilder<RelCourseTag> typeBuilder)
        {
            typeBuilder.ToTable("CranRelCourseTag");

            typeBuilder.HasOne(x => x.Course)
                .WithMany(c => c.RelTags)
                .HasForeignKey(rel => rel.IdCourse);

            typeBuilder.HasOne(x => x.Tag)
                .WithMany().HasForeignKey(rel => rel.IdTag);
        }

        private void MapRelQuestionTag(EntityTypeBuilder<RelQuestionTag> typeBuilder)
        {
            typeBuilder.ToTable("CranRelQuestionTag");

            typeBuilder.HasOne(x => x.Question)
                .WithMany(c => c.RelTags)
                .HasForeignKey(rel => rel.IdQuestion);

            typeBuilder.HasOne(x => x.Tag)
                .WithMany()
                .HasForeignKey(rel => rel.IdTag);
        }


    }
}
