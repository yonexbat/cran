using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Principal;
using System.Threading.Tasks;
using cran.Data;
using cran.Model.Dto;
using cran.Model.Entities;
using cran.Services.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace cran.Services
{
    public class TagService : CraniumService, ITagService
    {
        private readonly ICacheService _cacheService;
        private readonly ISecurityService _securityService;
        private readonly ApplicationDbContext _dbContext;

        public TagService(ApplicationDbContext context, IDbLogService dbLogService, IPrincipal principal,
            ICacheService cacheService, ISecurityService securityService) : base(context, dbLogService, securityService)
        {
            _cacheService = cacheService;
            _securityService = securityService;
            _dbContext = context;
        }

        public async Task<TagDto> GetTagAsync(int id)
        {
            Tag tag = await _dbContext.FindAsync<Tag>(id);
            if(tag == null)
            {
                throw new EntityNotFoundException(id, typeof(Tag));
            }
            return ToTagDto(tag);            
        }

        public async Task UpdateTagAsync(TagDto vm)
        {
            Tag tag = await _dbContext.FindAsync<Tag>(vm.Id);
            UpdateEntity(tag, vm);          
            await SaveChangesAsync();
        }

        public async Task<int> InsertTagAsync(TagDto dto)
        {
            Tag tag = new Tag();
            tag.TagType = TagType.Standard;
            UpdateEntity(tag, dto);
            _dbContext.Tags.Add(tag);
            await SaveChangesAsync();
            return tag.Id;           
        }

        private void UpdateEntity(Tag entity, TagDto dto)
        {
            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.ShortDescDe = dto.ShortDescDe;
            entity.ShortDescEn = dto.ShortDescEn;
        }

        public async Task<IList<TagDto>> FindTagsAsync(string searchTerm)
        {
            IQueryable<Tag> tagQueryable = _dbContext.Tags
                .Where(x => x.Name.Contains(searchTerm))
                .Where(x => x.TagType == TagType.Standard)
                .OrderBy(x => x.Name);
            IList<TagDto> result = await ToDto(tagQueryable);         
            return result;
        }

        public async Task<PagedResultDto<TagDto>> SearchForTagsAsync(SearchTags parameters)
        {
            IQueryable<Tag> query = _dbContext.Tags
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Id);
            if(!string.IsNullOrWhiteSpace(parameters.Name))
            {
                query = query.Where(x => x.Name.Contains(parameters.Name));
            }

            PagedResultDto<TagDto> result = await ToPagedResult(query, parameters.Page, ToDto);
            return result;

        }
       
        public async Task DeleteTagAsync(int id)
        {
            if(!_securityService.IsInRole(Roles.Admin))
            {
                throw new SecurityException($"No rights to delete a tag");
            }

            Tag tag = await _dbContext.FindAsync<Tag>(id);
            
            //Tags question
            IList<RelQuestionTag> relQuestionTags = await _dbContext.RelQuestionTags.Where(x => x.Tag.Id == id).ToListAsync(); 
            foreach(RelQuestionTag relQuestionTag in relQuestionTags)
            {
                _dbContext.Remove(relQuestionTag);
            }

            //Tags Course
            IList<RelCourseTag> relCourseTags = await _dbContext.RelCourseTags.Where(x => x.Tag.Id == id).ToListAsync();
            foreach (RelCourseTag relcourseTag in relCourseTags)
            {
                _dbContext.Remove(relcourseTag);
            }

            _dbContext.Remove(tag);

            await SaveChangesAsync();
        }

        public async Task<IList<TagDto>> GetTagsAsync(IList<int> ids)
        {
            IEnumerable<int> idsEnumearble = ids.AsEnumerable();
            IQueryable<Tag> query = _dbContext.Tags
                .Where(x => idsEnumearble.Contains(x.Id))
                .OrderBy(x => x.Name);
            IList<TagDto> tags = await ToDto(query);
            return tags;
        }

        private async Task<IList<TagDto>> ToDto(IQueryable<Tag> query)
        {
            return await query.Select(x => new TagDto {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    ShortDescDe = x.ShortDescDe,
                    IdTagType = (int) x.TagType,
                    ShortDescEn = x.ShortDescEn
                })
                .ToListAsync();
        }

        private TagDto ToTagDto(Tag tag)
        {
            return new TagDto
            {
                Id = tag.Id,
                Description = tag.Description,
                Name = tag.Name,
                ShortDescDe = tag.ShortDescDe,
                ShortDescEn = tag.ShortDescEn,
                IdTagType = (int) tag.TagType,
            };
        }

        public async Task<TagDto> GetSpecialTagAsync(SpecialTag specialTag)
        {
            switch(specialTag)
            {
                case SpecialTag.Deprecated:
                    return await _cacheService.GetEntryAsync("TagDeprecated", GetDeprecatedTag);
                default:
                    throw new CraniumException($"Special tag {specialTag} not supported");
            }
        }

        private async Task<TagDto> GetDeprecatedTag()
        {
            Tag tag =  await _dbContext.Tags.Where(x => x.Name == "Deprecated").SingleAsync();
            return ToTagDto(tag);
        }
    }
}
