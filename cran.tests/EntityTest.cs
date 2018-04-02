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
    /// <summary>
    /// Test DB Mapping (on a real Database).
    /// </summary>
    public class EntityTest : Base
    {        

       

        [Fact]
        public async Task TestQuestion()
        {
            //Prepare
            ApplicationDbContext context = CreateDbContext(GetConfiguration());


            Container container = new Container();
            context.Containers.Add(container);

            Question question1 = new Question();
            question1.Container = container;
            question1.Title = "hello";
            question1.Text = "Hello Text";
            question1.Status = QuestionStatus.Created;
            question1.User = await GetTestUserAsync(context);
            question1.Language = Language.De;
            context.Questions.Add(question1);
                   

            //Act
            context.SaveChanges();

            //Assert
            Question found = await context.FindAsync<Question>(question1.Id);
            Assert.NotNull(found);

            //Cleanup
            context.Remove(question1);
            context.Remove(container);
            context.SaveChanges();

        }

       
    }
}
