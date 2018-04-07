using cran.Data;
using cran.Model.Dto;
using cran.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            context.AddPrinicpalmock();
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
            answer.Answers.Add(true);
            answer.Answers.Add(false);

            var result5 = await courseInstanceService.AnswerQuestionAndGetSolutionAsync(answer);


            var result6 = await courseInstanceService.AnswerQuestionAndGetNextQuestionAsync(answer);
        }

    }
}
