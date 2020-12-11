using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using cran.Data;
using cran.Model.Entities;
using cran.Model.Dto;
using Microsoft.EntityFrameworkCore;
using cran.Mappers;
using cran.Services.Util;

namespace cran.Services
{
    public class CourseService : ICourseService
    {

        private readonly ISecurityService _securityService;
        private readonly ApplicationDbContext _dbContext;
        private readonly IDbLogService _dbLogService;

        public CourseService(
            ApplicationDbContext context, 
            IDbLogService dbLogService, 
            ISecurityService securityService)
        {
            _securityService = securityService;
            _dbContext = context;
            _dbLogService = dbLogService;
        }

        public async Task<CourseDto> GetCourseAsync(int id)
        {
            Course course = await this._dbContext.Courses
                .Where(x => x.Id == id)
                .Include(x => x.RelTags)
                .ThenInclude(x => x.Tag)
                .SingleAsync();

            CourseDto result = await ToCourseDto(course);
            return result;
        }

        public async Task<PagedResultDto<CourseDto>> GetCoursesAsync(int page)
        {
            IQueryable<Course> query = this._dbContext.Courses
                .OrderBy(x => x.Title)
                .ThenBy(x => x.Id);

            return await PagedResultUtil.ToPagedResult(query, page, ToDto);
        }

        private async Task<IList<CourseDto>> ToDto(IQueryable<Course> query)
        {
            query = query.Include(x => x.RelTags)
               .ThenInclude(x => x.Tag);

            IList<Course> list = await query
               .Include(x => x.RelTags)
               .ThenInclude(x => x.Tag)
               .ToListAsync();
            return await ListUtil.ToDtoListAsync(list, ToCourseDto);
        }

     

        private async Task<CourseDto> ToCourseDto(Course course)
        {
            string userid = _securityService.GetUserId();
            bool isFavorite = await _dbContext.RelUserCourseFavorites
                .AnyAsync(x => x.Course.Id == course.Id && x.User.UserId == userid);
            bool isEditable = _securityService.IsInRole(Roles.Admin);
            return course.Map(isEditable, isFavorite);
        }

        public async Task<int> InsertCourseAsync(CourseDto courseDto)
        {
            await _dbLogService.LogMessageAsync("Adding course");

            Course entity = new Course();
            CopyDataCourse(courseDto, entity);

            await _dbContext.AddAsync(entity);

            await _dbContext.SaveChangesAsync();
            courseDto.Id = entity.Id;
            await UpdateCourseAsync(courseDto);

            return courseDto.Id;
        }

        public async Task UpdateCourseAsync(CourseDto courseDto)
        {

            Course courseEntity = await this._dbContext.FindAsync<Course>(courseDto.Id);

            //Tags
            IList<RelCourseTag> relTagEntities = await _dbContext.RelCourseTags
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
            _dbContext.UpdateRelation(relCourseTagDtos, relTagEntities, CopyDataRelCourse);

            CopyDataCourse(courseDto, courseEntity);

            await _dbContext.SaveChangesAsync();
        }

        private void CopyDataRelCourse(RelCourseTagDto dto, RelCourseTag entity)
        {
            RelCourseTagDto dtoSource = dto;
            RelCourseTag entityDestination = entity;
            entityDestination.IdCourse = dtoSource.IdCourse;
            entityDestination.IdTag = dtoSource.IdTag;
        }

        private void CopyDataCourse(CourseDto dto, Course entity)
        {
            entity.Title = dto.Title;
            entity.Language = Enum.Parse<Language>(dto.Language);
            entity.NumQuestionsToAsk = dto.NumQuestionsToAsk;
            entity.Description = dto.Description;
        }
 
    }
}
