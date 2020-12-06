using cran.Data;
using cran.Model.Dto;
using cran.Model.Entities;
using cran.Services.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace cran.Services
{
    public class VersionService : CraniumService, IVersionService
    {
        private IQuestionService _questionService;
        private ITagService _tagService;
        private INotificationService _notificationService;
        private ITextService _textService;
        private readonly ISecurityService _securityService;

        public VersionService(ApplicationDbContext context,
            IDbLogService dbLogService,
            ISecurityService securityService,
            IQuestionService questionService,
            ITagService tagService,
            INotificationService notificationService,
            ITextService textService) : base(context, dbLogService, securityService)
        {
            _questionService = questionService;
            _tagService = tagService;
            _notificationService = notificationService;
            _textService = textService;
            _securityService = securityService;
        }

        public async Task<int> CopyQuestionAsync(int id)
        {
            QuestionDto questionDto = await CreateQuestionDtoCopy(id);
            int newId = await _questionService.InsertQuestionAsync(questionDto);
            Question questionNew = await _context.FindAsync<Question>(newId);
            questionNew.IdQuestionCopySource = id;
            await SaveChangesAsync();
            return newId;
        }

        public async Task<int> VersionQuestionAsync(int id)
        {
            //security check
            await _questionService.CheckWriteAccessToQuestion(id);

            Question questionSourceEntity = await _context.FindAsync<Question>(id);
            if (questionSourceEntity.Status != QuestionStatus.Released)
            {
                new CraniumException($"Question #{id} is not in state released.");
            }
            QuestionDto questionDto = await CreateQuestionDtoCopy(questionSourceEntity.Id);

            //Create new Question
            Question newQuestion = new Question();
            newQuestion.Container = questionSourceEntity.Container;
            newQuestion.IdContainer = questionSourceEntity.IdContainer;
            newQuestion.User = questionSourceEntity.User;
            newQuestion.IdQuestionCopySource = id;
            CopyData(questionDto, newQuestion);

            await _context.Questions.AddAsync(newQuestion);
            await SaveChangesAsync();

            //Copy all data                    
            questionDto.Id = newQuestion.Id;
            await _questionService.UpdateQuestionAsync(questionDto);

            return newQuestion.Id;
        }

        private async Task<QuestionDto> CreateQuestionDtoCopy(int id)
        {
            QuestionDto questionDto = await _questionService.GetQuestionAsync(id);
            questionDto.Status = (int)QuestionStatus.Created;
            foreach (QuestionOptionDto option in questionDto.Options)
            {
                option.Id = 0;
            }
            questionDto.Tags = questionDto.Tags.Where(x => x.IdTagType == (int)TagType.Standard)
                .ToList();
            questionDto.Id = 0;
            return questionDto;
        }

        public async Task AcceptQuestionAsync(int id)
        {
            //security check
            await _questionService.CheckWriteAccessToQuestion(id);

            Question question = await _context.FindAsync<Question>(id);
            if (question.Status != QuestionStatus.Created)
            {
                new CraniumException($"Question #{id} is not in state created.");
            }
            question.Status = QuestionStatus.Released;
            question.ApprovalDate = DateTime.Now;

            IList<Question> previousQuestions = await _context.Questions
                .Where(x => x.IdContainer == question.IdContainer)
                .Where(x => x.Status == QuestionStatus.Released)
                .Where(x => x.Id != id)
                .Include(x => x.RelTags)
                .ToListAsync();

            TagDto deprecatedTag = await _tagService.GetSpecialTagAsync(SpecialTag.Deprecated);
            Tag deprecatedTagEntity = await _context.FindAsync<Tag>(deprecatedTag.Id);

            foreach (Question previousQuestion in previousQuestions)
            {
                previousQuestion.Status = QuestionStatus.Obsolete;
                if (!previousQuestion.RelTags.Any(x => x.IdTag == deprecatedTag.Id))
                {
                    RelQuestionTag relQuestionTag = new RelQuestionTag
                    {
                        Question = previousQuestion,
                        Tag = deprecatedTagEntity,
                    };
                    await _context.RelQuestionTags.AddAsync(relQuestionTag);
                }
            }

            await _context.SaveChangesAsync();
            await SendNotification(id, "QuestionAcceptTitle", "QuestionAcceptText");
            
        }

        private async Task SendNotification(int id, string title, string text)
        {
            Question question = await _context.FindAsync<Question>(id);
            title = await _textService.GetTextAsync(title, question.Title);
            text = await _textService.GetTextAsync(text, question.Title);
            await _notificationService.SendNotificationAboutQuestionAsync(id, title, text);
        }


        public async Task<PagedResultDto<VersionInfoDto>> GetVersionsAsync(VersionInfoParametersDto versionInfoParameters)
        {
            Question questionEntity  = await _context.FindAsync<Question>(versionInfoParameters.IdQuestion);

            IQueryable<Question> query = _context.Questions.Where(x => x.Container.Id == questionEntity.IdContainer)
                .OrderByDescending(x => x.Id);

            return await ToPagedResult(query, versionInfoParameters.Page, MaterializeQuestionList);
        }

        private async Task<IList<VersionInfoDto>> MaterializeQuestionList(IQueryable<Question> query)
        {
            IList<VersionInfoDto> result = await query
                .Select(x => new VersionInfoDto()
                {
                    IdQuestion = x.Id,
                    User = x.User.UserId,
                    InsertDate = x.InsertDate,
                    Status = x.Status,
                    ApprovalDate = x.ApprovalDate,
                })
                .ToListAsync();
            return result;
        }
    }
}
