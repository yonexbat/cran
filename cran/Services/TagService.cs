using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using cran.Data;
using cran.Model.Dto;
using cran.Model.Entities;
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
            return new TagDto
            {
                Id = tag.Id,
                Description = tag.Description,
                Name = tag.Name,
            };
        }

        public async Task UpdateTagAsync(TagDto vm)
        {
            Tag tag = await _context.FindAsync<Tag>(vm.Id);
            tag.Name = vm.Name;
            tag.Description = vm.Description;
            await SaveChangesAsync();
        }

        public async Task<int> InsertTagAsync(TagDto vm)
        {

            Tag tag = new Tag();
            tag.Name = vm.Name;
            tag.Description = vm.Description;
            _context.Tags.Add(tag);
            await SaveChangesAsync();
            return tag.Id;           
        }

        public async Task<IList<TagDto>> FindTagsAsync(string searchTerm)
        {
            IList<Tag> tags = await _context.Tags.Where(x => x.Name.Contains(searchTerm)).ToListAsync();
            IList<TagDto> result = new List<TagDto>();

            foreach (Tag tag in tags)
            {
                TagDto tagVm = new TagDto
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    Description = tag.Description,
                };
                result.Add(tagVm);
            }
            return result;
        }

        public async Task<PagedResultDto<TagDto>> SearchForTags(SearchTags parameters)
        {
            IQueryable<Tag> queryBeforeSkipAndTake = _context.Tags
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Id);

            if (!string.IsNullOrWhiteSpace(parameters.Name))
            {
                queryBeforeSkipAndTake = queryBeforeSkipAndTake.Where(x => x.Name.Contains(parameters.Name));
            }

            PagedResultDto<TagDto> resultDto = new PagedResultDto<TagDto>();

            //Count und paging.
            int count = await queryBeforeSkipAndTake.CountAsync();
            int startindex = InitPagedResult(resultDto, count, parameters.Page);

            //Daten 
            IQueryable<Tag> query = queryBeforeSkipAndTake.Skip(startindex).Take(PageSize);

            resultDto.Data = await ToDto(query);

            return resultDto;
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
            return await query.Select(x => new TagDto { Id = x.Id, Name = x.Name, Description = x.Description })
                .ToListAsync();
        }

    }
}
