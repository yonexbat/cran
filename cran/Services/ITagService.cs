using cran.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cran.Services
{
    public interface ITagService
    {
        Task<TagDto> GetTagAsync(int id);
        Task UpdateTagAsync(TagDto vm);
        Task<int> InsertTagAsync(TagDto vm);
        Task<IList<TagDto>> FindTagsAsync(string searchTerm);
        Task<PagedResultDto<TagDto>> SearchForTags(SearchTags parameters);
        Task DeleteTagAsync(int id);
    }
}
