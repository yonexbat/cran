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
    public class CourseInstanceServiceTest : Base
    {

        protected ICourseService _courseService;

        protected override void SetUpDependencies(IDictionary<Type, object> dependencyMap)
        {
            base.SetUpDependencies(dependencyMap);
            
            dependencyMap[typeof(ICommentsService)] = GetServiceInMemoryDb<CommentsService>();
            dependencyMap[typeof(ITextService)] = GetServiceInMemoryDb<TextService>();
            dependencyMap[typeof(IQuestionService)] = GetServiceInMemoryDb<QuestionService>();

            

            _courseService = GetServiceInMemoryDb<CourseService>();
            dependencyMap[typeof(ICourseService)] = _courseService;
        }


        [Fact]
        public async Task TestStartTest()
        {
            
            ICourseInstanceService courseInstanceService = GetServiceInMemoryDb<CourseInstanceService>();

           
            var courses = await _courseService.GetCoursesAsync(0);
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
