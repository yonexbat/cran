using cran.Data;
using cran.Model.Entities;
using cran.Model.Dto;
using cran.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.Options;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace cran.tests.Infra
{
    public class TestingContext
    {
        readonly IDictionary<Type, object> _dependencyMap = new Dictionary<Type, object>();

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
              .UseLazyLoadingProxies()
              .Options;

            IPrincipal principal = GetSimple<IPrincipal>();

            ApplicationDbContext context = new ApplicationDbContext(options, principal);
            InMemoryDbSetup setup = new InMemoryDbSetup();
            setup.SetUpInMemoryDb(context);

            _dependencyMap[context.GetType()] = context;
        }

        public void AddInfoTextServiceMock()
        {
            var infoMock = new Mock<ITextService>(MockBehavior.Loose);
            infoMock.Setup(x => x.GetTextAsync(It.IsAny<string>(), It.IsAny<string[]>()))
                    .Returns((string key, string[] paramsarray) => Task.FromResult<string>(key));
            _dependencyMap[typeof(ITextService)] = infoMock.Object;
        }

        public void AddUserService()
        {
            _dependencyMap[typeof(IUserService)] = GetService<UserService>();
        }

        public void AddCranAppSettings()
        {
           IOptions<CranSettingsDto> settings =  Microsoft.Extensions.Options.Options.Create(new CranSettingsDto()
           {

           });
           _dependencyMap[typeof(IOptions<CranSettingsDto>)] = settings;
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
            string connString = config["CranSettings:ConnectionString"];
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(connString)
            .UseLazyLoadingProxies()
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

            ISecurityService securityService = new SecurityService(principal);
            _dependencyMap[typeof(ISecurityService)] = securityService;
        }

        public void AddLogServiceMock()
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
            ICacheService cacheService = GetService<CacheService>();
            cacheService.Clear();
            _dependencyMap[typeof(ICacheService)] = cacheService;
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
