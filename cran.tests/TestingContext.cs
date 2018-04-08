using cran.Data;
using cran.Model.Entities;
using cran.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

[assembly: UserSecretsId("CRANSECRETS201707021036")]

namespace cran.tests
{
    public class TestingContext
    {
        IDictionary<Type, object> _dependencyMap = new Dictionary<Type, object>();

        

        public IDictionary<Type, object> DependencyMap => _dependencyMap;

        public void AddBinaryServiceMock()
        {
            Stream result = new MemoryStream();
            byte[] helloWorld = Encoding.UTF8.GetBytes("helloworld");
            result.Write(helloWorld, 0, helloWorld.Length);
            result.Seek(0, SeekOrigin.Begin);

            var binaryMock = new Mock<IBinaryService>(MockBehavior.Loose);
            binaryMock.Setup(x => x.GetBinaryAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(result));

            _dependencyMap[typeof(IBinaryService)] = binaryMock.Object;
        }

        public void AddAdminPrincipalMock()
        {
            var identityMock = new Mock<IIdentity>(MockBehavior.Loose);
            identityMock.Setup(x => x.Name).Returns("testuser");
            IIdentity identity = identityMock.Object;

            var pricipalMock = new Mock<IPrincipal>(MockBehavior.Loose);
            pricipalMock.Setup(x => x.Identity).Returns(identity);
            pricipalMock.Setup(x => x.IsInRole(Roles.Admin)).Returns(true);
            pricipalMock.Setup(x => x.IsInRole(Roles.User)).Returns(true);
            IPrincipal principal = pricipalMock.Object;
            _dependencyMap[typeof(IPrincipal)] = principal;
        }

        

        public void AddInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: "CraniumInMemoryContext")
              .Options;

            IPrincipal principal = GetSimple<IPrincipal>();

            ApplicationDbContext context = new ApplicationDbContext(options, principal);
            InitInMemoryDb(context);

            _dependencyMap[context.GetType()] = context;
        }

        public void AddRealDb()
        {
            IConfiguration config = GetConfiguration();
            IPrincipal principal = GetSimple<IPrincipal>();
            ApplicationDbContext context = CreateDbContext(config, principal);
            _dependencyMap[context.GetType()] =  context;
        }

        protected IConfiguration GetConfiguration()
        {
            // Adding JSON file into IConfiguration.
            IConfiguration config = new ConfigurationBuilder()
                .AddUserSecrets<TestingContext>()
                .Build();

            return config;
        }

        protected ApplicationDbContext CreateDbContext(IConfiguration config, IPrincipal principal)
        {
            string connString = config["ConnectionString"];
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(connString)
            .Options;
            ApplicationDbContext context = new ApplicationDbContext(options, principal);
            return context;
        }

    

        public void AddPrinicpalmock()
        {
            var identityMock = new Mock<IIdentity>(MockBehavior.Loose);
            identityMock.Setup(x => x.Name).Returns("testuser");
            IIdentity identity = identityMock.Object;

            var pricipalMock = new Mock<IPrincipal>(MockBehavior.Loose);
            pricipalMock.Setup(x => x.Identity).Returns(identity);
            IPrincipal principal = pricipalMock.Object;
            _dependencyMap[typeof(IPrincipal)] = principal;
        }

        public void AddMockLogService()
        {
            IDbLogService log = new Mock<IDbLogService>(MockBehavior.Loose).Object;
            _dependencyMap[typeof(IDbLogService)] = log;
        }

        protected virtual void InitInMemoryDb(ApplicationDbContext context)
        {

            context.Database.EnsureDeleted();

            //Tags
            IList<Tag> tags = new List<Tag>();
            for (int i = 1; i <= 10; i++)
            {
                Tag tag = new Tag()
                {
                    Description = $"Description{i}",
                    TagType = TagType.Standard,
                };
                context.Tags.Add(tag);
                tags.Add(tag);
            };
            context.SaveChanges();

            //Questions
            for (int i = 1; i <= 10; i++)
            {
                CreateMockQuestion(context, i, tags);
            }

            //Courses
            Course course = new Course()
            {
                Description = $"Description",
                Title = $"Title",
                NumQuestionsToAsk = 5,
                Language = Language.De,
            };
            context.Courses.Add(course);
            RelCourseTag relCourseTag = new RelCourseTag()
            {
                Course = course,
                Tag = tags.First(),
            };
            context.RelCourseTags.Add(relCourseTag);
            context.SaveChanges();
        }


        public T GetService<T>() where T : class
        {
            var testingObject = new TestingObject<T>();           
            foreach (var mapEntry in _dependencyMap)
            {
                testingObject.DependencyMap[mapEntry.Key] = mapEntry.Value;
            }
            return testingObject.GetResolvedTestingObject();
        }

        public T GetSimple<T>()
        {
            Type typeParameterType = typeof(T);
            return (T) _dependencyMap[typeParameterType];
        }

        protected virtual void CreateMockQuestion(ApplicationDbContext context, int id, IList<Tag> tags)
        {
            Question question = new Question()
            {
                Explanation = $"Explanation{id}",
                Text = $"Text{id}",
                User = new CranUser() { UserId = $"UserId{id}", },
                Container = new Container() { },
                Status = QuestionStatus.Released,
                Language = Language.De,
            };
            context.Questions.Add(question);

            //Options
            for (int i = 1; i <= 4; i++)
            {
                QuestionOption option = new QuestionOption()
                {
                    IdQuestion = question.Id,
                    Text = $"OptionText{i}",
                    IsTrue = i % 2 == 0,
                    Question = question,
                };
                question.Options.Add(option);
                context.QuestionOptions.Add(option);
            }

            //Tags
            for (int i = 1; i <= 4; i++)
            {
                RelQuestionTag relTag = new RelQuestionTag
                {
                    Question = question,
                    Tag = tags[i - 1],
                };
                context.RelQuestionTags.Add(relTag);
            }

            //Binary
            for (int i = 1; i <= 3; i++)
            {
                Binary binary = new Binary()
                {
                    ContentType = "image/png",
                    FileName = $"Filename{i + id * 1000}",
                    ContentDisposition = $"form-data; name=\"files\"; filename=\"Untitled.png\"",
                    Length = 20618,
                };
                context.Binaries.Add(binary);

                Image image = new Image()
                {
                    Binary = binary,
                    Height = 300,
                };
                context.Images.Add(image);

                RelQuestionImage relQuestionImage = new RelQuestionImage
                {
                    Question = question,
                    Image = image,
                };
                context.RelQuestionImages.Add(relQuestionImage);
            }

            context.SaveChanges();
        }

    }
}
