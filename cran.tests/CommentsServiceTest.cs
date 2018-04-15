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
    public class CommentsServiceTest
    {

        private void SetUpTestingContext(TestingContext testingContext)
        {
            testingContext.AddPrincipalMock();
            testingContext.AddBinaryServiceMock();
            testingContext.AddMockLogService();
            testingContext.AddInMemoryDb();
        }

        [Fact]
        public async Task TestAddComment()
        {
            TestingContext testingContext = new TestingContext();

            ApplicationDbContext appDbContext = testingContext.GetSimple<ApplicationDbContext>();
            ICommentsService commentsService = testingContext.GetService<CommentsService>();

            Question question = appDbContext.Questions.First();
            CommentDto commentDto = new CommentDto()
            {
                IdQuestion = question.Id,
                CommentText = "Hello Comment",
            };

            //Act
            int newId = await commentsService.AddCommentAsync(commentDto);
            Assert.True(newId > 0);
        }

        [Fact]
        public async Task TestGetComments()
        {
            TestingContext testingContext = new TestingContext();
            SetUpTestingContext(testingContext);

            ApplicationDbContext appDbContext = testingContext.GetSimple<ApplicationDbContext>();
            ICommentsService commentsService = testingContext.GetService<CommentsService>();

            Question question = appDbContext.Questions.First();
            for (int i = 0; i < 20; i++)
            {
                CommentDto commentDto = new CommentDto()
                {
                    IdQuestion = question.Id,
                    CommentText = "Hello Comment",
                };
                await commentsService.AddCommentAsync(commentDto);
            }
            GetCommentsDto getCommentsDto = new GetCommentsDto
            {
                IdQuestion = question.Id,
                Page = 0,
            };

            //Act
            PagedResultDto<CommentDto> comments = await commentsService.GetCommentssAsync(getCommentsDto);

            //Assert
            Assert.Equal(5, comments.Data.Count);
            Assert.Equal(4, comments.Numpages);
            Assert.Equal(20, comments.Count);
        }
    }
}
