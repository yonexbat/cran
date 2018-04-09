using cran.Data;
using cran.Model.Entities;
using cran.tests.Infra;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace cran.tests
{
    /// <summary>
    /// Test DB Mapping (on a real Database).
    /// </summary>
    public class EntityTest
    {        

       

        [Fact]
        public async Task TestQuestion()
        {
            //Prepare
            TestingContext testingContext = new TestingContext();
            testingContext.AddAdminPrincipalMock();
            testingContext.AddMockLogService();
            testingContext.AddRealDb();


            ApplicationDbContext context = testingContext.GetSimple<ApplicationDbContext>();


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

        [Fact]
        public async Task TestTag()
        {
            //Prepare
            TestingContext testingContext = new TestingContext();
            testingContext.AddAdminPrincipalMock();
            testingContext.AddMockLogService();
            testingContext.AddRealDb();


            ApplicationDbContext context = testingContext.GetSimple<ApplicationDbContext>();

            Tag tag = new Tag
            {
                Name = "Test",
                TagType = TagType.Standard,
                Description = "Desc",
                ShortDescDe = "ShortDescDe",
                ShortDescEn = "ShortDescEn",
            };

            context.Tags.Add(tag);

            //Act
            await context.SaveChangesAsync();

            //Assert
            Assert.True(tag.Id > 0);

            //Cleanup        
            context.Remove(tag);            
            context.SaveChanges();
        }

        protected Task<CranUser> GetTestUserAsync(ApplicationDbContext context)
        {
            return context.CranUsers.Where(x => x.UserId == "testuser").SingleAsync();
        }

    }
}
