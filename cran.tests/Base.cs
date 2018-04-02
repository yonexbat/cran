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
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Xunit;


namespace cran.tests
{
    public class Base
    {


        protected T GetService<T>() where T : class
        {
            IConfiguration config = GetConfiguration();
            ApplicationDbContext context = CreateDbContext(config);

            var testingObject = new TestingObject<T>();
            testingObject.AddDependency(context);
            testingObject.AddDependency(new Mock<IDbLogService>(MockBehavior.Loose));
            testingObject.AddDependency(GetPrincipalMock());
            return testingObject.GetResolvedTestingObject();
        }

        protected IConfiguration GetConfiguration()
        {
            // Adding JSON file into IConfiguration.
            IConfiguration config = new ConfigurationBuilder()
                .AddUserSecrets<CraniumServiceTest>()
                .Build();

            return config;
        }       


        protected ApplicationDbContext CreateDbContext(IConfiguration config)
        {
            string connString = config["ConnectionString"];
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(connString)
            .Options;
            ApplicationDbContext context = new ApplicationDbContext(options, GetPrincipalMock());
            return context;
        }


        protected ApplicationDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "CraniumInMemoryContext")
                .Options;
            ApplicationDbContext context = new ApplicationDbContext(options, GetPrincipalMock());
            InitInMemoryDb(context);
            return context;
        }


        protected virtual void InitInMemoryDb(ApplicationDbContext context)
        {
            //Tags
            for (int i = 1; i <= 4; i++)
            {
                Tag tag = new Tag()
                {
                    Id = i,
                    Description = $"Description{i}",
                };
                context.Tags.Add(tag);
            };
            context.SaveChanges();

            for (int i=1; i<=100; i++)
            {
                CreateMockQuestion(context, i);
            }
        }

        protected virtual void CreateMockQuestion(ApplicationDbContext context, int id)
        {
            Question question = new Question()
            {
                Id = id,
                Explanation = $"Explanation{id}",
                Text = $"Text{id}",
                IdUser = id,
                User = new CranUser() {Id = id,UserId = $"UserId{id}", },
                Container = new Container() { Id = id,},    
                Status = QuestionStatus.Released,
            };
            context.Questions.Add(question);

            //Options
            for(int i=1; i<=4; i++)
            {
                QuestionOption option = new QuestionOption()
                {
                    Id = i + id * 1000,
                    IdQuestion = id,
                    Text = $"OptionText{i}",
                    IsTrue = i % 2 == 0,
                    Question = question,
                };
                question.Options.Add(option);
                context.QuestionOptions.Add(option);

            }

            //Tags
            for(int i=1; i<=4; i++)
            {
                RelQuestionTag relTag = new RelQuestionTag
                {
                    Id = i + id * 1000,
                    IdTag = i,
                    IdQuestion = id,
                };
                context.RelQuestionTags.Add(relTag);
            }

            //Binary
            for(int i=1;i<=3; i++)
            {
                Binary binary = new Binary()
                {
                    Id = i + id * 1000,
                    ContentType = "image/png",
                    FileName = $"Filename{i + id * 1000}",
                    ContentDisposition = $"form-data; name=\"files\"; filename=\"Untitled.png\"",
                    Length = 20618,
                };
                context.Binaries.Add(binary);

                Image image = new Image()
                {
                    Id = i + id * 1000,
                    Binary = binary,
                    Height = 300,
                };
                context.Images.Add(image);

                RelQuestionImage relQuestionImage = new RelQuestionImage
                {
                    Id = i + id * 1000,
                    Question = question,
                    Image = image,
                };
                context.RelQuestionImages.Add(relQuestionImage);
            }
           

            context.SaveChanges();
        }

        protected IPrincipal GetPrincipalMock()
        {
            var identityMock = new Mock<IIdentity>(MockBehavior.Loose);
            identityMock.Setup(x => x.Name).Returns("testuser");
            IIdentity identity = identityMock.Object;

            var pricipalMock = new Mock<IPrincipal>(MockBehavior.Loose);
            pricipalMock.Setup(x => x.Identity).Returns(identity);
            IPrincipal principal = pricipalMock.Object;
            return principal;
        }

        protected IPrincipal GetPricipalAdminMock()
        {
            var identityMock = new Mock<IIdentity>(MockBehavior.Loose);
            identityMock.Setup(x => x.Name).Returns("testuser");
            IIdentity identity = identityMock.Object;

            var pricipalMock = new Mock<IPrincipal>(MockBehavior.Loose);
            pricipalMock.Setup(x => x.Identity).Returns(identity);
            pricipalMock.Setup(x => x.IsInRole(Roles.Admin)).Returns(true);
            pricipalMock.Setup(x => x.IsInRole(Roles.User)).Returns(true);
            IPrincipal principal = pricipalMock.Object;
            return principal;
        }

        protected IBinaryService CreateBinaryServiceMock()
        {
            Stream result = new MemoryStream();
            byte[] helloWorld = Encoding.UTF8.GetBytes("helloworld");
            result.Write(helloWorld, 0, helloWorld.Length);
            result.Seek(0, SeekOrigin.Begin);

            var binaryMock = new Mock<IBinaryService>(MockBehavior.Loose);
            binaryMock.Setup(x => x.GetBinaryAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(result));
            return binaryMock.Object;
        }


        protected Task<CranUser> GetTestUserAsync(ApplicationDbContext context)
        {
            return context.CranUsers.Where(x => x.UserId == "testuser").SingleAsync();
        }
        
    }
}
