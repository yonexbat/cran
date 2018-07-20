using cran.Data;
using cran.Model.Dto;
using cran.Model.Entities;
using cran.Services;
using cran.tests.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace cran.tests
{
    public class VersionServiceTest
    {

        private void InitContext(TestingContext context)
        {
            context.AddPrincipalMock();
            context.AddInMemoryDb();
            context.AddMockLogService();
            context.AddGermanCultureServiceMock();
            context.AddBinaryServiceMock();
            context.AddCacheService();
            context.DependencyMap[typeof(ICommentsService)] = context.GetService<CommentsService>();
            context.DependencyMap[typeof(ITagService)] = context.GetService<TagService>();
        }

        [Fact]
        public async Task TestVersionQuestion()
        {
            //Prepare
            TestingContext context = new TestingContext();
            InitContext(context);
            ApplicationDbContext dbContext = context.GetSimple<ApplicationDbContext>();
            Question question = dbContext.Questions.First();
            context.AddPrincipalMock(question.User.UserId, Roles.User);           

            IQuestionService questionService = context.GetService<QuestionService>();
            context.DependencyMap[typeof(IQuestionService)] = questionService;
            IVersionService versionService = context.GetService<VersionService>();

            //Act
            int newId = await versionService.VersionQuestionAsync(question.Id);

            //Assert
            Assert.True(question.Id != newId);
            Assert.True(newId > 0);

            await versionService.AcceptQuestionAsync(newId);

            //Assert
            QuestionDto oldDto = await questionService.GetQuestionAsync(question.Id);
            QuestionDto newDto = await questionService.GetQuestionAsync(newId);
            Assert.Contains(oldDto.Tags, x => x.Name == "Deprecated");
            Assert.Equal(QuestionStatus.Released, (QuestionStatus)newDto.Status);
            Assert.Equal(QuestionStatus.Obsolete, (QuestionStatus)oldDto.Status);
            question = await dbContext.FindAsync<Question>(newId);
            Assert.NotNull(question.ApprovalDate);

        }

        [Fact]
        public async Task TestGetVersions()
        {
            //Prepare
            TestingContext context = new TestingContext();
            InitContext(context);
            ApplicationDbContext dbContext = context.GetSimple<ApplicationDbContext>();
            Question question = dbContext.Questions.First();
            context.AddPrincipalMock(question.User.UserId, Roles.User);

            IQuestionService questionService = context.GetService<QuestionService>();
            context.DependencyMap[typeof(IQuestionService)] = questionService;
            IVersionService versionService = context.GetService<VersionService>();

            
            int newId = await versionService.VersionQuestionAsync(question.Id);           
            await versionService.AcceptQuestionAsync(newId);

            VersionInfoParametersDto versionInfoParametersDto = new VersionInfoParametersDto()
            {
                Page = 0,
                IdQuestion = newId,
            };

            //Act
            PagedResultDto<VersionInfoDto> result = await versionService.GetVersionsAsync(versionInfoParametersDto);

            //Assert
            Assert.Equal(2, result.Count);

        }

        [Fact]
        public async Task TestCopyQuestion()
        {
            //Prepare
            TestingContext context = new TestingContext();
            InitContext(context);
            ApplicationDbContext dbContext = context.GetSimple<ApplicationDbContext>();
            Question question = dbContext.Questions.First();
            context.AddPrincipalMock(question.User.UserId, Roles.User);

            IQuestionService questionService = context.GetService<QuestionService>();
            context.DependencyMap[typeof(IQuestionService)] = questionService;
            IVersionService versionService = context.GetService<VersionService>();

            //Act
            int newId = await versionService.CopyQuestionAsync(question.Id);

            //Assert
            Assert.True(question.Id != newId);
            Assert.True(newId > 0);
            QuestionDto newQuestion = await questionService.GetQuestionAsync(newId);
            Assert.Equal(question.Options.Count, newQuestion.Options.Count);
            Assert.Equal(question.QuestionType, newQuestion.QuestonType);
            for (int i = 0; i < question.Options.Count; i++)
            {
                QuestionOption optionSource = question.Options[i];
                QuestionOptionDto optionDestination = newQuestion.Options[i];

                Assert.NotEqual(optionSource.Id, optionDestination.Id);
                Assert.True(optionDestination.Id > 0);
            }

        }
    }
}
