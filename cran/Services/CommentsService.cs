using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using cran.Data;
using cran.Model.Dto;
using cran.Model.Entities;
using System.Security;
using Microsoft.EntityFrameworkCore;

namespace cran.Services
{
    public class CommentsService : CraniumService, ICommentsService
    {
        public CommentsService(ApplicationDbContext context, IDbLogService dbLogService, IPrincipal principal) : base(context, dbLogService, principal)
        {
        }

        public async Task<VotesDto> VoteAsync(VotesDto vote)
        {
            string userId = this.GetUserId();
            Rating rating = _context.Ratings
                .Where(x => x.User.UserId == userId && x.Question.Id == vote.IdQuestion)
                .SingleOrDefault();
            if (rating == null)
            {
                Question question = await _context.FindAsync<Question>(vote.IdQuestion);
                CranUser user = await GetCranUserAsync();
                rating = new Rating
                {
                    Question = question,
                    User = user,
                    QuestionRating = 0,
                };
                _context.Ratings.Add(rating);
            }

            int r = vote.MyVote > 0 ? 1 : (vote.MyVote < 0 ? -1 : 0);
            rating.QuestionRating = r;
            await SaveChangesAsync();
            vote = await GetVoteAsync(vote.IdQuestion);
            return vote;
        }

        public async Task<VotesDto> GetVoteAsync(int idQuestion)
        {

            int upRatings = await _context.Ratings.Where(x => x.Question.Id == idQuestion && x.QuestionRating > 0)
                .CountAsync();
            int downRatings = await _context.Ratings.Where(x => x.Question.Id == idQuestion && x.QuestionRating < 0)
                .CountAsync();

            string userId = this.GetUserId();
            int? myRating = await _context.Ratings.Where(x => x.User.UserId == userId && x.Question.Id == idQuestion)
                .Select(x => x.QuestionRating).SingleOrDefaultAsync();

            VotesDto result = new VotesDto()
            {
                IdQuestion = idQuestion,
                MyVote = myRating ?? 0,
                DownVotes = downRatings,
                UpVotes = upRatings,
            };

            return result;
        }

        public async Task<int> AddCommentAsync(CommentDto vm)
        {
            CranUser cranUser = await this.GetCranUserAsync();
            Question question = await _context.FindAsync<Question>(vm.IdQuestion);
            Comment comment = new Comment()
            {
                Question = question,
                User = cranUser,
                CommentText = vm.CommentText,
            };
            _context.Comments.Add(comment);
            await this.SaveChangesAsync();
            return comment.Id;
        }

        public async Task DeleteCommentAsync(int id)
        {
            Comment comment = await _context.FindAsync<Comment>(id);
            if (!(await HasWriteAccess(comment.IdUser)))
            {
                throw new SecurityException($"No access to comment,  id: {id}");
            }
            _context.Remove(comment);
            await this.SaveChangesAsync();
        }


        public async Task<PagedResultDto<CommentDto>> GetCommentssAsync(GetCommentsDto parameters)
        {
            PagedResultDto<CommentDto> resultDto = new PagedResultDto<CommentDto>();

            IQueryable<Comment> queryBeforeSkipAndTake = _context.Comments
                .Where(x => x.Question.Id == parameters.IdQuestion)
                .OrderByDescending(x => x.InsertDate)
                .ThenBy(x => x.Id);

            PagedResultDto<CommentDto> result = await ToPagedResult(queryBeforeSkipAndTake, parameters.Page, ToDto);
            return result;
        }

        private async Task<IList<CommentDto>> ToDto(IQueryable<Comment> query)
        {            
            var data = await query.Select(x => new
            {
                x.CommentText,
                IdQuestion = x.Question.Id,
                IdUser = x.User.Id,
                x.User.UserId,
                x.InsertDate,
                x.UpdateDate,
                IdComment = x.Id,

            }).ToListAsync();

            IList<CommentDto> result = new List<CommentDto>();

            foreach (var commentData in data)
            {
                CommentDto commentDto = new CommentDto
                {
                    IdComment = commentData.IdComment,
                    CommentText = commentData.CommentText,
                    IdQuestion = commentData.IdQuestion,
                    UserId = commentData.UserId,
                    InsertDate = commentData.InsertDate,
                    UpdateDate = commentData.UpdateDate,
                    IsEditable = await HasWriteAccess(commentData.IdUser),
                };
                result.Add(commentDto);
            }

            return result;
        }
        
    }
}
