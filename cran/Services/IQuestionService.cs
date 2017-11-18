using cran.Model.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cran.Services
{
    public interface IQuestionService
    {
        Task<int> InsertQuestionAsync(QuestionDto question);
        Task UpdateQuestionAsync(QuestionDto question);
        Task DeleteQuestionAsync(int idQuestion);
        Task<IList<QuestionListEntryDto>> GetMyQuestionsAsync();
        Task<QuestionDto> GetQuestionAsync(int id);
        Task<PagedResultDto<QuestionListEntryDto>> SearchForQuestionsAsync(SearchQParametersDto parameters);
        Task<ImageDto> AddImageAsync(ImageDto imageDto);
    }

}
