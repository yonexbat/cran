using cran.Data;
using cran.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using Moq;
using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Xunit;

[assembly: UserSecretsId("CRANSECRETS201707021036")]

namespace cran.tests
{    
    

    public class CraniumServiceTest
    {

        protected IConfiguration GetConfiguration()
        {
            // Adding JSON file into IConfiguration.
            IConfiguration config = new ConfigurationBuilder()
                .AddUserSecrets<CraniumServiceTest>()
                .Build();            

            return config;
        }

        private ApplicationDbContext CreateDbContext(IConfiguration config)
        {            
            string connString = config["ConnectionString"];
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(connString)
            .Options;
            ApplicationDbContext context = new ApplicationDbContext(options);
            return context;
        }

        private TestingObject<CraniumService> GetTestingObject()
        {
            IConfiguration config = GetConfiguration();
            ApplicationDbContext context = CreateDbContext(config);        

            var identityMock = new Mock<IIdentity>(MockBehavior.Loose);
            identityMock.Setup(x => x.Name).Returns("testuser");
            IIdentity identity = identityMock.Object;

            var pricipalMock = new Mock<IPrincipal>(MockBehavior.Loose);
            pricipalMock.Setup(x => x.Identity).Returns(identity);
            IPrincipal principal = pricipalMock.Object;

            var testingObject = new TestingObject<CraniumService>();
            testingObject.AddDependency(context);
            testingObject.AddDependency(new Mock<IDbLogService>(MockBehavior.Loose));
            testingObject.AddDependency(principal);
            return testingObject;
        }

        [Fact]
        public async Task TestStartTest()
        {
            var testignObject = GetTestingObject();
            ICraniumService service = testignObject.GetResolvedTestingObject();
            var courses = await service.CoursesAsync();
            var result = await service.StartCourseAsync(courses.Courses[0].Id);
        }
    }
}
