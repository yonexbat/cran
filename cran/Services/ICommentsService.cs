using cran.Model.Dto;
using System.Threading.Tasks;

namespace cran.Services
{
    public interface ICommentsService
    {
        Task<PagedResultDto<CommentDto>> GetCommentssAsync(GetCommentsDto parameters);
        Task<int> AddCommentAsync(CommentDto vm);
        Task DeleteCommentAsync(int id);
        Task<VotesDto> VoteAsync(VotesDto vote);
        Task<VotesDto> GetVoteAsync(int idQuestion);
    }
}
