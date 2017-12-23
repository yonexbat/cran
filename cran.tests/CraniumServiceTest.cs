using cran.Data;
using cran.Model.Dto;
using cran.Model.ViewModel;
using cran.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using Moq;
using System;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Xunit;

[assembly: UserSecretsId("CRANSECRETS201707021036")]

namespace cran.tests
{


    public class CraniumServiceTest : Base
    {
       

        private T GetService<T>() where T : class
        {
            IConfiguration config = GetConfiguration();
            ApplicationDbContext context = CreateDbContext(config);
            
            var testingObject = new TestingObject<T>();
            testingObject.AddDependency(context);
            testingObject.AddDependency(new Mock<IDbLogService>(MockBehavior.Loose));
            testingObject.AddDependency(GetPrincipalMock());
            return testingObject.GetResolvedTestingObject();
        }   

        private ICourseInstanceService CourseInstanceService()
        {
            IQuestionService questionService = QuestionService();

            IConfiguration config = GetConfiguration();
            ApplicationDbContext context = CreateDbContext(config);

            var testingObject = new TestingObject<CourseInstanceService>();
            testingObject.AddDependency(context);
            testingObject.AddDependency(new Mock<IDbLogService>(MockBehavior.Loose));
            testingObject.AddDependency(GetPrincipalMock());
            testingObject.AddDependency(questionService);
            return testingObject.GetResolvedTestingObject();
        }

        private IQuestionService QuestionService()
        {
            ICommentsService commentsService = GetService<CommentsService>();

            IConfiguration config = GetConfiguration();
            ApplicationDbContext context = CreateDbContext(config);

            var testingObject = new TestingObject<QuestionService>();
            testingObject.AddDependency(context);
            testingObject.AddDependency(new Mock<IDbLogService>(MockBehavior.Loose));
            testingObject.AddDependency(GetPrincipalMock());
            testingObject.AddDependency(commentsService);
            return testingObject.GetResolvedTestingObject();
        }

        [Fact]
        public async Task TestStartTest()
        {

            ICourseService courseService = GetService<CourseService>();
            ICourseInstanceService courseInstanceService = CourseInstanceService();

            var courses = await courseService.GetCoursesAsync(0);
            int courseId = courses.Data.Where(x => x.Title == "JS").Select(x => x.Id).First();
            var result = await courseInstanceService.StartCourseAsync(courseId);
            Assert.True(result.IdCourseInstance > 0);
            Assert.True(result.IdCourse == courseId);
            Assert.True(result.IdCourseInstanceQuestion > 0);
            Assert.True(result.NumQuestionsAlreadyAsked == 0);
            Assert.True(result.NumQuestionsTotal > 0);

            var result2 = await courseInstanceService.NextQuestion(result.IdCourseInstance);
            Assert.True(result2.IdCourse == courseId);
            Assert.True(result2.IdCourseInstanceQuestion > 0);
            Assert.True(result2.NumQuestionsAlreadyAsked == 1);
            Assert.True(result2.NumQuestionsTotal > 0);

            var result3 = await courseInstanceService.NextQuestion(result.IdCourseInstance);
            Assert.True(result3.IdCourse == courseId);
            Assert.True(result3.IdCourseInstanceQuestion > 0);
            Assert.True(result3.NumQuestionsAlreadyAsked == 2);
            Assert.True(result3.NumQuestionsTotal > 0);

            var result4 = await courseInstanceService.GetQuestionToAskAsync(result.IdCourseInstanceQuestion);

            QuestionAnswerDto answer = new QuestionAnswerDto();
            answer.IdCourseInstanceQuestion = result.IdCourseInstanceQuestion;
            answer.Answers.Add(true);
            answer.Answers.Add(false);

            var result5 = await courseInstanceService.AnswerQuestionAndGetSolutionAsync(answer);


            var result6 = await courseInstanceService.AnswerQuestionAndGetNextQuestionAsync(answer);
        }
       

        [Fact]
        public async Task TestAddImage()
        {

            IQuestionService questionService = QuestionService();


            //Add Q
            QuestionDto qdto = new QuestionDto()
            {
                Title = "Bla",
                Explanation = "bla",
            };
            int  newId = await questionService.InsertQuestionAsync(qdto);

            //Add Binary
            IBinaryService binaryService = GetService<BinaryService>();
            int id = await binaryService.AddBinaryAsync(new BinaryDto
            {
                ContentDisposition = "ContentDisposition",
                ContentType = "ContentType",
                FileName = "FileName",
                Name = "Name",
                Length = 2334,
            });


            ImageDto imageDto = new ImageDto()
            {
                IdBinary = id,
                Full = false,
                Height = 124,
                Width = 64,
            };          

            imageDto = await questionService.AddImageAsync(imageDto);
            QuestionDto questionDto = await questionService.GetQuestionAsync(newId);
            questionDto.Images.Add(imageDto);

            await questionService.UpdateQuestionAsync(questionDto);

            questionDto = await questionService.GetQuestionAsync(newId);

            Assert.True(questionDto.Images.Count == 1);

        }
    }
}
