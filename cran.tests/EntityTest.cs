using cran.Data;
using cran.Model.Dto;
using cran.Model.Entities;
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

namespace cran.tests
{
    public class EntityTest : Base
    {        

       

        [Fact]
        public async Task TestQuestionSuccessor()
        {
            ApplicationDbContext context = CreateDbContext(GetConfiguration());

            Question question1 = new Question();
            question1.Title = "hello";
            question1.Text = "Hello Text";
            question1.Status = QuestionStatus.Created;
            question1.User = await GetTestUserAsync(context);
            question1.Language = Language.De;
            context.Questions.Add(question1);
            context.SaveChanges();

            Question question2 = new Question();
            question2.Title = "hello";
            question2.Text = "Hello Text";
            question2.Status = QuestionStatus.Created;
            question2.User = await GetTestUserAsync(context);
            question2.Language = Language.De;
            context.Questions.Add(question2);
            context.SaveChanges();

            question1.Successor = question2;

            context.SaveChanges();

        }

        [Fact]
        public async Task TestQuestionPredecessor()
        {
            ApplicationDbContext context = CreateDbContext(GetConfiguration());

            Question question1 = new Question();
            question1.Title = "hello";
            question1.Text = "Hello Text";
            question1.Status = QuestionStatus.Created;
            question1.User = await GetTestUserAsync(context);
            question1.Language = Language.De;
            context.Questions.Add(question1);
            context.SaveChanges();

            Question question2 = new Question();
            question2.Title = "hello";
            question2.Text = "Hello Text";
            question2.Status = QuestionStatus.Created;
            question2.User = await GetTestUserAsync(context);
            question2.Language = Language.De;
            context.Questions.Add(question2);
            context.SaveChanges();

            question2.Predecessor = question1;

            context.SaveChanges();

        }
    }
}
