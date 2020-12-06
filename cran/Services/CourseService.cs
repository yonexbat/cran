﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using cran.Data;
using cran.Model.Entities;
using cran.Model.Dto;
using Microsoft.EntityFrameworkCore;
using cran.Mappers;

namespace cran.Services
{
    public class CourseService : CraniumService, ICourseService
    {

        private readonly ISecurityService _securityService;
        private readonly ApplicationDbContext _dbContext;

        public CourseService(ApplicationDbContext context, IDbLogService dbLogService, ISecurityService securityService) : base(context, dbLogService, securityService)
        {
            _securityService = securityService;
            _dbContext = context;
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

            return await ToPagedResult(query, page, ToDto);
        }

        private async Task<IList<CourseDto>> ToDto(IQueryable<Course> query)
        {
            query = query.Include(x => x.RelTags)
               .ThenInclude(x => x.Tag);

            IList<Course> list = await query
               .Include(x => x.RelTags)
               .ThenInclude(x => x.Tag)
               .ToListAsync();
            return await ToDtoListAsync(list, ToCourseDto);
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
            CopyData(courseDto, entity);

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
            UpdateRelation(relCourseTagDtos, relTagEntities);

            CopyData(courseDto, courseEntity);

            await _dbContext.SaveChangesAsync();
        }
    }
}
