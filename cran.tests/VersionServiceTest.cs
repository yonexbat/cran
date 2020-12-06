using cran.Data;
using cran.Model.Dto;
using cran.Model.Entities;
using cran.Services;
using cran.tests.Infra;
using Moq;
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

        private void SetUpTestingContext(TestingContext testingContext)
        {
            testingContext.AddPrincipalMock();
            testingContext.AddInMemoryDb();
            testingContext.AddUserService();
            testingContext.AddBusinessSecurityService();
            testingContext.AddLogServiceMock();
            testingContext.AddGermanCultureServiceMock();
            testingContext.AddBinaryServiceMock();
            testingContext.AddInfoTextServiceMock();
            testingContext.AddCacheService();
            testingContext.DependencyMap[typeof(ICommentsService)] = testingContext.GetService<CommentsService>();
            testingContext.DependencyMap[typeof(ITagService)] = testingContext.GetService<TagService>();

             
             
            Mock<INotificationService> notificationMock =  new Mock<INotificationService>(MockBehavior.Loose);
            notificationMock.Setup(x => x.SendNotificationAboutQuestionAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            
            testingContext.DependencyMap[typeof(INotificationService)] = notificationMock.Object;
        }

       

        [Fact]
        public async Task TestVersionQuestion()
        {
            //Prepare
            TestingContext testingContext = new TestingContext();
            SetUpTestingContext(testingContext);
            ApplicationDbContext dbContext = testingContext.GetSimple<ApplicationDbContext>();
            Question question = dbContext.Questions.First();
            testingContext.AddPrincipalMock(question.User.UserId, Roles.User);
            testingContext.AddBusinessSecurityService();

            IQuestionService questionService = testingContext.GetService<QuestionService>();
            testingContext.DependencyMap[typeof(IQuestionService)] = questionService;

             Mock<INotificationService> notificationMock =  new Mock<INotificationService>(MockBehavior.Loose);
            notificationMock.Setup(x => x.SendNotificationAboutQuestionAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            testingContext.DependencyMap[typeof(INotificationService)] = notificationMock.Object;

            IVersionService versionService = testingContext.GetService<VersionService>();

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
            notificationMock.Verify(x => x.SendNotificationAboutQuestionAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once());

        }

        [Fact]
        public async Task TestGetVersions()
        {
            //Prepare
            TestingContext testingContext = new TestingContext();
            SetUpTestingContext(testingContext);
            ApplicationDbContext dbContext = testingContext.GetSimple<ApplicationDbContext>();
            Question question = dbContext.Questions.First();
            testingContext.AddPrincipalMock(question.User.UserId, Roles.User);
            testingContext.AddBusinessSecurityService();

            IQuestionService questionService = testingContext.GetService<QuestionService>();
            testingContext.DependencyMap[typeof(IQuestionService)] = questionService;
            IVersionService versionService = testingContext.GetService<VersionService>();

            
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
            TestingContext testingContext = new TestingContext();
            SetUpTestingContext(testingContext);
            ApplicationDbContext dbContext = testingContext.GetSimple<ApplicationDbContext>();
            Question question = dbContext.Questions.First();
            testingContext.AddPrincipalMock(question.User.UserId, Roles.User);
            testingContext.AddBusinessSecurityService();
            testingContext.AddUserService();

            IQuestionService questionService = testingContext.GetService<QuestionService>();
            testingContext.DependencyMap[typeof(IQuestionService)] = questionService;           
            IVersionService versionService = testingContext.GetService<VersionService>();

            //Act
            int newId = await versionService.CopyQuestionAsync(question.Id);

            //Assert
            Assert.True(question.Id != newId);
            Assert.True(newId > 0);
            QuestionDto newQuestion = await questionService.GetQuestionAsync(newId);
            Assert.Equal(question.Options.Count, newQuestion.Options.Count);
            Assert.Equal(question.QuestionType, newQuestion.QuestionType);
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
