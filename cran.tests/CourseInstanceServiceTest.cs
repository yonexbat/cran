using cran.Data;
using cran.Model.Dto;
using cran.Model.Entities;
using cran.Services;
using cran.tests.Infra;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace cran.tests
{
    public class CourseInstanceServiceTest
    {

        [Fact]
        public async Task TestStartTest()
        {
            TestingContext context = new TestingContext();
            context.AddPrincipalMock();
            context.AddBinaryServiceMock();
            context.AddInMemoryDb();
            context.AddMockLogService();
            context.DependencyMap[typeof(ITextService)] = context.GetService<TextService>();
            context.DependencyMap[typeof(ICommentsService)] = context.GetService<CommentsService>();
            context.DependencyMap[typeof(IQuestionService)] = context.GetService<QuestionService>();
            ICourseService courseService = context.GetService<CourseService>();
            context.DependencyMap[typeof(ICourseService)] = courseService;

            ICourseInstanceService courseInstanceService = context.GetService<CourseInstanceService>();


            var courses = await courseService.GetCoursesAsync(0);
            int courseId = courses.Data.Select(x => x.Id).First();

            //Act
            var result = await courseInstanceService.StartCourseAsync(courseId);
            Assert.True(result.IdCourseInstance > 0);
            Assert.True(result.IdCourse == courseId);
            Assert.True(result.IdCourseInstanceQuestion > 0);
            Assert.True(result.NumQuestionsAlreadyAsked == 0);
            Assert.True(result.NumQuestionsTotal > 0);

            CourseInstanceDto result2 = await courseInstanceService.NextQuestion(result.IdCourseInstance);
            Assert.True(result2.IdCourse == courseId);
            Assert.True(result2.IdCourseInstanceQuestion > 0);
            Assert.True(result2.NumQuestionsAlreadyAsked == 1);
            Assert.True(result2.NumQuestionsTotal > 0);

            CourseInstanceDto result3 = await courseInstanceService.NextQuestion(result.IdCourseInstance);
            Assert.True(result3.IdCourse == courseId);
            Assert.True(result3.IdCourseInstanceQuestion > 0);
            Assert.True(result3.NumQuestionsAlreadyAsked == 2);
            Assert.True(result3.NumQuestionsTotal > 0);

            QuestionToAskDto result4 = await courseInstanceService.GetQuestionToAskAsync(result.IdCourseInstanceQuestion);

            QuestionAnswerDto answer = new QuestionAnswerDto();
            answer.IdCourseInstanceQuestion = result.IdCourseInstanceQuestion;
            answer.Answers.Add(false);
            answer.Answers.Add(true);
            answer.Answers.Add(false);
            answer.Answers.Add(true);

            QuestionDto result5 = await courseInstanceService.AnswerQuestionAndGetSolutionAsync(answer);
            Assert.True(result5.Explanation != null);
            Assert.Equal(4, result5.Options.Count);
            Assert.True(result5.Options[1].IsTrue);
            Assert.True(result5.Options[3].IsTrue);


            CourseInstanceDto result6 = await courseInstanceService.AnswerQuestionAndGetNextQuestionAsync(answer);
            Assert.True(result6.AnsweredCorrectly);
            Assert.True(result6.Done);
            Assert.Equal(3, result6.NumQuestionsAlreadyAsked);
            Assert.Equal(3, result6.NumQuestionsTotal);

        }


        [Fact]
        public async Task TestNotEnoughQuestion()
        {
            //Prepare
            TestingContext context = new TestingContext();
            context.AddPrincipalMock();
            context.AddBinaryServiceMock();
            context.AddInMemoryDb();
            context.AddMockLogService();
            context.DependencyMap[typeof(ITextService)] = context.GetService<TextService>();
            context.DependencyMap[typeof(ICommentsService)] = context.GetService<CommentsService>();
            context.DependencyMap[typeof(IQuestionService)] = context.GetService<QuestionService>();
            CourseService courseService = context.GetService<CourseService>();
            context.DependencyMap[typeof(ICourseService)] = courseService;

            ApplicationDbContext dbContext = context.GetSimple<ApplicationDbContext>();
            Course course = dbContext.Courses.First();
            course.NumQuestionsToAsk = 100;

            ICourseInstanceService courseInstanceService = context.GetService<CourseInstanceService>();

            //Act
            var courseInstance = await courseInstanceService.StartCourseAsync(course.Id);

            //Assert
            Assert.Equal(10, courseInstance.NumQuestionsTotal);
            
        }
    }
}
