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
using Xunit;

[assembly: UserSecretsId("CRANSECRETS201707021036")]
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace cran.tests.Infra
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

       

        public void AddInMemoryDb()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
              .UseInMemoryDatabase(databaseName: "CraniumInMemoryContext")
              .Options;

            IPrincipal principal = GetSimple<IPrincipal>();

            ApplicationDbContext context = new ApplicationDbContext(options, principal);
            InMemoryDbSetup setup = new InMemoryDbSetup();
            setup.SetUpInMemoryDb(context);

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

        public void AddPrincipalMock()
        {
            AddPrincipalMock("testuser", Roles.User);
        }

        public void AddAdminPrincipalMock()
        {
            AddPrincipalMock("testuser", Roles.User, Roles.Admin);           
        }

        public void AddPrincipalMock(string name, params string[] roles)
        {
            var identityMock = new Mock<IIdentity>(MockBehavior.Loose);
            identityMock.Setup(x => x.Name).Returns(name);
            IIdentity identity = identityMock.Object;

            var pricipalMock = new Mock<IPrincipal>(MockBehavior.Loose);
            pricipalMock.Setup(x => x.Identity).Returns(identity);
            foreach(string role in roles)
            {
                pricipalMock.Setup(x => x.IsInRole(role)).Returns(true);
            }          
            IPrincipal principal = pricipalMock.Object;
            _dependencyMap[typeof(IPrincipal)] = principal;
        }

        public void AddMockLogService()
        {
            IDbLogService log = new Mock<IDbLogService>(MockBehavior.Loose).Object;
            _dependencyMap[typeof(IDbLogService)] = log;
        }

        public void AddCacheService()
        {
            _dependencyMap[typeof(ICacheService)] = GetService<CacheService>();
        }

        public void AddGermanCultureServiceMock()
        {
            var mock = new Mock<ICultureService>(MockBehavior.Loose);
            mock.Setup(x => x.GetCurrentLanguage()).Returns(Language.De);
            ICultureService cultureService = mock.Object;
            _dependencyMap[typeof(ICultureService)] = cultureService;
        }

        public void AddQuestionService()
        {
            _dependencyMap[typeof(ICacheService)] = GetService<CacheService>();
            _dependencyMap[typeof(ITagService)] = GetService<TagService>();
            _dependencyMap[typeof(ICommentsService)] = GetService<CommentsService>();
            _dependencyMap[typeof(IQuestionService)] = GetService<QuestionService>();
        }


        public void AddEnglishCultureServiceMock()
        {
            var mock = new Mock<ICultureService>(MockBehavior.Loose);
            mock.Setup(x => x.GetCurrentLanguage()).Returns(Language.En);
            ICultureService cultureService = mock.Object;
            _dependencyMap[typeof(ICultureService)] = cultureService;
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

      

    }
}
