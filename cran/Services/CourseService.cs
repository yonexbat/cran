using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using cran.Data;
using cran.Model.Entities;
using cran.Model.Dto;
using Microsoft.EntityFrameworkCore;

namespace cran.Services
{
    public class CourseService : CraniumService, ICourseService
    {
        public CourseService(ApplicationDbContext context, IDbLogService dbLogService, IPrincipal principal) : base(context, dbLogService, principal)
        {
        }

        public async Task<CourseDto> GetCourseAsync(int id)
        {
            Course course = await this._context.Courses
                .Where(x => x.Id == id)
                .Include(x => x.RelTags)
                .ThenInclude(x => x.Tag)
                .SingleAsync();

            CourseDto result = ToCourseDto(course);
            return result;
        }

        public async Task<CoursesDto> GetCoursesAsync()
        {
            await _dbLogService.LogMessageAsync("courses");
            CoursesDto result = new CoursesDto();
            IList<Course> list = await this._context.Courses
                .Include(x => x.RelTags)
                .ThenInclude(x => x.Tag)
                .ToListAsync();

            foreach (Course course in list)
            {
                CourseDto courseVm = ToCourseDto(course);
                result.Courses.Add(courseVm);
            }
            return result;
        }

        private CourseDto ToCourseDto(Course course)
        {
            CourseDto courseVm = new CourseDto
            {
                Id = course.Id,
                Title = course.Title,
                Language = course.Language.ToString(),
                Description = course.Description,
                NumQuestionsToAsk = course.NumQuestionsToAsk,
                IsEditable = _currentPrincipal.IsInRole(Roles.Admin),
            };

            foreach (RelCourseTag relTag in course.RelTags)
            {
                Tag tag = relTag.Tag;
                TagDto tagVm = new TagDto
                {
                    Id = tag.Id,
                    Description = tag.Description,
                    Name = tag.Name,
                };
                courseVm.Tags.Add(tagVm);
            }

            return courseVm;
        }

        public async Task<InsertActionDto> InsertCourseAsync(CourseDto courseDto)
        {
            await _dbLogService.LogMessageAsync("Adding course");

            Course entity = new Course();
            CopyData(courseDto, entity);

            _context.Add(entity);

            await SaveChangesAsync();
            courseDto.Id = entity.Id;
            await UpdateCourseAsync(courseDto);

            return new InsertActionDto
            {
                NewId = courseDto.Id,
                Status = "Ok",
            };
        }

        public async Task UpdateCourseAsync(CourseDto courseDto)
        {

            Course courseEntity = await this._context.FindAsync<Course>(courseDto.Id);

            //Tags
            IList<RelCourseTag> relTagEntities = await _context.RelCourseTags
                .Where(x => x.IdCourse == courseEntity.Id).ToListAsync();
            relTagEntities = relTagEntities.GroupBy(x => x.IdTag).Select(x => x.First()).ToList();
            IDictionary<int, int> relIdByTagId = relTagEntities.ToDictionary(x => x.IdTag, x => x.Id);
            IList<RelCourseTagDto> relCourseTagDtos = new List<RelCourseTagDto>();
            IList<TagDto> tagDtos = courseDto.Tags.GroupBy(x => x.Id).Select(x => x.First()).ToList();

            foreach (TagDto tagDto in tagDtos)
            {
                RelCourseTagDto relCourseTag = new RelCourseTagDto();
                relCourseTag.IdTag = tagDto.Id;
                relCourseTag.IdCourse = courseDto.Id;
                if (relIdByTagId.ContainsKey(tagDto.Id))
                {
                    relCourseTag.Id = relIdByTagId[tagDto.Id];
                }

                relCourseTagDtos.Add(relCourseTag);
            }
            UpdateRelation(relCourseTagDtos, relTagEntities);

            CopyData(courseDto, courseEntity);

            await _context.SaveChangesCranAsync(_currentPrincipal);
        }
    }
}
