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
        public DbSet<Container> Containers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionOption> QuestionOptions { get; set; }
        public DbSet<RelQuestionTag> RelQuestionTags { get; set; }
        public DbSet<RelCourseTag> RelCourseTags { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<CranUser> CranUsers { get; set; }
        public DbSet<CourseInstance> CourseInstances { get; set; }
        public DbSet<CourseInstanceQuestion> CourseInstancesQuestion { get; set; }
        public DbSet<CourseInstanceQuestionOption> CourseInstancesQuestionOption { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Binary> Binaries { get; set; }
        public DbSet<RelQuestionImage> RelQuestionImages { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Text> Texts { get; set; }

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
            MapComment(builder.Entity<Comment>());
            MapRating(builder.Entity<Rating>());
            MapRelQuestionImage(builder.Entity<RelQuestionImage>());
            MapBinary(builder.Entity<Binary>());
            MapImage(builder.Entity<Image>());
            MapContainer(builder.Entity<Container>());
            MapText(builder.Entity<Text>());
        }

        private void MapText(EntityTypeBuilder<Text> typeBuilder)
        {
            typeBuilder.ToTable("CranText");
        }

        private void MapContainer(EntityTypeBuilder<Container> typeBuilder)
        {
            typeBuilder.ToTable("CranContainer");

            typeBuilder
               .HasMany(x => x.Questions)
               .WithOne(o => o.Container)
               .HasForeignKey(x => x.IdContainer);

        }

        private void MapImage(EntityTypeBuilder<Image> typeBuilder)
        {
            typeBuilder.ToTable("CranImage");

            typeBuilder.HasOne(x => x.Binary)
                .WithMany(x => x.Images)
                .HasForeignKey(x => x.IdBinary);

            typeBuilder.HasOne(x => x.RelImage)
                .WithOne(x => x.Image)
                .HasForeignKey<RelQuestionImage>(x => x.IdImage);
        }        

        private void MapBinary(EntityTypeBuilder<Binary> typeBuilder)
        {
            typeBuilder.ToTable("CranBinary");

            typeBuilder.HasOne(binary => binary.User)
                .WithMany(user => user.Binaries)
                .HasForeignKey(binary => binary.IdUser);

            typeBuilder
               .HasMany(binary => binary.Images)
               .WithOne(image => image.Binary)
               .HasForeignKey(image => image.IdBinary);
        }

        private void MapRelQuestionImage(EntityTypeBuilder<RelQuestionImage> typeBuilder)
        {
            typeBuilder.ToTable("CranRelQuestionImage");

            typeBuilder.HasOne(x => x.Question)
               .WithMany(c => c.RelImages)
               .HasForeignKey(rel => rel.IdQuestion);

            typeBuilder.HasOne(x => x.Image)
                .WithOne(x => x.RelImage)
                .HasForeignKey<RelQuestionImage>(x => x.IdImage);
                
        }

        private void MapComment(EntityTypeBuilder<Comment> typeBuilder)
        {
            typeBuilder.ToTable("CranComment");

            typeBuilder.HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.IdUser);

            typeBuilder.HasOne(c => c.Question)
             .WithMany(q => q.Comments)
             .HasForeignKey(c => c.IdQuestion);

        }

        private void MapRating(EntityTypeBuilder<Rating> typeBuilder)
        {
            typeBuilder.ToTable("CranRating");

            typeBuilder.HasOne(r => r.User)
                 .WithMany(u => u.Ratings)
                 .HasForeignKey(c => c.IdUser);

            typeBuilder.HasOne(r => r.Question)
             .WithMany(q => q.Ratings)
             .HasForeignKey(c => c.IdQuestion);
        }

        private void MapCranUser(EntityTypeBuilder<CranUser> typeBuilder)
        {
            typeBuilder.Property(b => b.Id)
                .ValueGeneratedOnAdd();

            typeBuilder.ToTable("CranUser");

            typeBuilder.HasMany(u => u.Questions)
                .WithOne(q => q.User)
                .HasForeignKey(q => q.IdUser);
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

            typeBuilder.Property(c => c.Language)
           .HasColumnName("IdLanguage");
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

            typeBuilder
                .HasOne(x => x.QuestionCopySource)
                .WithMany(x => x.CopiedQuestions)
                .HasForeignKey(x => x.IdQuestionCopySource);
                                                

            typeBuilder
                .HasMany(x => x.RelImages)
                .WithOne(o => o.Question)
                .HasForeignKey(x => x.IdQuestion);

            typeBuilder
                .HasOne(q => q.User)
                .WithMany(u => u.Questions)
                .HasForeignKey(q => q.IdUser);

            typeBuilder
                .HasOne(q => q.Container)
                .WithMany(u => u.Questions)
                .HasForeignKey(q => q.IdContainer);

            typeBuilder.Property(q => q.Language)
            .HasColumnName("IdLanguage");

        }

        private void MapTag(EntityTypeBuilder<Tag> typeBuilder)
        {
            typeBuilder.ToTable("CranTag");

            typeBuilder.Property(q => q.TagType)
           .HasColumnName("IdTagType");
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
