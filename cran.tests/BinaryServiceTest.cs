using cran.Data;
using cran.Model.Dto;
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
using System.Threading.Tasks;
using Xunit;


namespace cran.tests
{    
    

    public class BinaryServiceTest : Base
    {

      

        private TestingObject<BinaryService> GetTestingObject()
        {
            IConfiguration config = GetConfiguration();
            ApplicationDbContext context = CreateDbContext(config);        

            var testingObject = new TestingObject<BinaryService>();
            testingObject.AddDependency(context);
            testingObject.AddDependency(new Mock<IDbLogService>(MockBehavior.Loose));
            testingObject.AddDependency(GetPrincipalMock());
            return testingObject;
        }

        [Fact]
        public async Task TestStartTest()
        {
            var testignObject = GetTestingObject();

            string testWord = "Hello World";
            var bytes = System.Text.Encoding.Unicode.GetBytes(testWord);

            MemoryStream stream = new MemoryStream();
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            IBinaryService service = testignObject.GetResolvedTestingObject();

            int id = await service.AddBinaryAsync(new BinaryDto
            {
                ContentDisposition = "ContentDisposition",
                ContentType = "ContentType",
                FileName = "FileName",
                Name = "Name",
                Length = 2334,
            });

            await service.SaveAsync(id, stream);

            RelQuestionImageDto dto = new RelQuestionImageDto
            {
                IdBinary = 1,
                IdQuestion = 1,
            };
            

        }   
    }
}
