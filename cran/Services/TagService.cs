using System;
using System.Collections.Generic;
using System.Linq;
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
        public TagService(ApplicationDbContext context, IDbLogService dbLogService, IPrincipal principal) : base(context, dbLogService, principal)
        {
        }

        public async Task<TagDto> GetTagAsync(int id)
        {
            Tag tag = await _context.FindAsync<Tag>(id);
            if(tag == null)
            {
                throw new EntityNotFoundException(id, typeof(Tag));
            }
            return ToTagDto(tag);            
        }

        public async Task UpdateTagAsync(TagDto vm)
        {
            Tag tag = await _context.FindAsync<Tag>(vm.Id);
            UpdateEntity(tag, vm);          
            await SaveChangesAsync();
        }

        public async Task<int> InsertTagAsync(TagDto dto)
        {
            Tag tag = new Tag();
            tag.TagType = TagType.Standard;
            UpdateEntity(tag, dto);
            _context.Tags.Add(tag);
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
            IQueryable<Tag> tagQueryable = _context.Tags.Where(x => x.Name.Contains(searchTerm))
                .OrderBy(x => x.Name);
            IList<TagDto> result = await ToDto(tagQueryable);         
            return result;
        }

        public async Task<PagedResultDto<TagDto>> SearchForTagsAsync(SearchTags parameters)
        {
            IQueryable<Tag> query = _context.Tags
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
            Tag tag = await _context.FindAsync<Tag>(id);
            
            //Tags question
            IList<RelQuestionTag> relQuestionTags = await _context.RelQuestionTags.Where(x => x.Tag.Id == id).ToListAsync(); 
            foreach(RelQuestionTag relQuestionTag in relQuestionTags)
            {
                _context.Remove(relQuestionTag);
            }

            //Tags Course
            IList<RelCourseTag> relCourseTags = await _context.RelCourseTags.Where(x => x.Tag.Id == id).ToListAsync();
            foreach (RelCourseTag relcourseTag in relCourseTags)
            {
                _context.Remove(relcourseTag);
            }

            _context.Remove(tag);

            await SaveChangesAsync();
        }

        public async Task<IList<TagDto>> GetTagsAsync(IList<int> ids)
        {
            IQueryable<Tag> query = _context.Tags
                .Where(x => ids.Contains(x.Id))
                .OrderBy(x => x.Name);
            IList<TagDto> tags = await ToDto(query);
            return tags;
        }

        private async Task<IList<TagDto>> ToDto(IQueryable<Tag> query)
        {
            return await query.Select(x => new TagDto {
                Id = x.Id, Name = x.Name,
                Description = x.Description,
                ShortDescDe = x.ShortDescDe,
                IdTagType = (int) x.TagType,
                ShortDescEn = x.ShortDescEn})
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
    }
}
