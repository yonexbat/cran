using cran.Model.Dto;
using System.Threading.Tasks;

namespace cran.Services
{
    public interface ITextService
    {
        Task<string> GetTextAsync(string key, params string[] placeholders);
        Task<TextDto> GetTextDtoAsync(int id);
        Task UpdateTextAsync(TextDto vm);
        Task<PagedResultDto<TextDto>> GetTextsAsync(SearchTextDto search);
    }
}
