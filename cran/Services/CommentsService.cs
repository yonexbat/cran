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
using cran.Services.Util;

namespace cran.Services
{
    public class CommentsService : CraniumService, ICommentsService
    {

        private readonly ISecurityService _securityService;
        private readonly ApplicationDbContext _dbContext;
        private readonly IUserService _userService;

        public CommentsService(ApplicationDbContext context, IDbLogService dbLogService, ISecurityService securityService, IUserService userService) : base(context, dbLogService, securityService)
        {
            _securityService = securityService;
            _dbContext = context;
            _userService = userService;
        }

        public async Task<VotesDto> VoteAsync(VotesDto vote)
        {
            string userId = _securityService.GetUserId();
            Rating rating = _dbContext.Ratings
                .Where(x => x.User.UserId == userId && x.Question.Id == vote.IdQuestion)
                .SingleOrDefault();
            if (rating == null)
            {
                Question question = await _dbContext.FindAsync<Question>(vote.IdQuestion);
                CranUser user = await _userService.GetOrCreateCranUserAsync();
                rating = new Rating
                {
                    Question = question,
                    User = user,
                    QuestionRating = 0,
                };
                _dbContext.Ratings.Add(rating);
            }

            int r = vote.MyVote > 0 ? 1 : (vote.MyVote < 0 ? -1 : 0);
            rating.QuestionRating = r;
            await _dbContext.SaveChangesAsync();
            vote = await GetVoteAsync(vote.IdQuestion);
            return vote;
        }

        public async Task<VotesDto> GetVoteAsync(int idQuestion)
        {

            int upRatings = await _dbContext.Ratings.Where(x => x.Question.Id == idQuestion && x.QuestionRating > 0)
                .CountAsync();
            int downRatings = await _dbContext.Ratings.Where(x => x.Question.Id == idQuestion && x.QuestionRating < 0)
                .CountAsync();

            string userId = this._securityService.GetUserId();
            int? myRating = await _dbContext.Ratings.Where(x => x.User.UserId == userId && x.Question.Id == idQuestion)
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
            CranUser cranUser = await _userService.GetOrCreateCranUserAsync();
            Question question = await _dbContext.FindAsync<Question>(vm.IdQuestion);
            Comment comment = new Comment()
            {
                Question = question,
                User = cranUser,
                CommentText = vm.CommentText,
            };
            _dbContext.Comments.Add(comment);
            await this._dbContext.SaveChangesAsync();
            return comment.Id;
        }

        public async Task DeleteCommentAsync(int id)
        {
            Comment comment = await _dbContext.FindAsync<Comment>(id);
            if (!(await HasWriteAccess(comment.IdUser)))
            {
                throw new SecurityException($"No access to comment,  id: {id}");
            }
            _dbContext.Remove(comment);
            await this._dbContext.SaveChangesAsync();
        }


        public async Task<PagedResultDto<CommentDto>> GetCommentssAsync(GetCommentsDto parameters)
        {
            PagedResultDto<CommentDto> resultDto = new PagedResultDto<CommentDto>();

            IQueryable<Comment> queryBeforeSkipAndTake = _dbContext.Comments
                .Where(x => x.Question.Id == parameters.IdQuestion)
                .OrderByDescending(x => x.InsertDate)
                .ThenBy(x => x.Id);

            PagedResultDto<CommentDto> result = await Util.PagedResultUtil.ToPagedResult(queryBeforeSkipAndTake, parameters.Page, ToDto);
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
