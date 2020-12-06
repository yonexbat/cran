using cran.Data;
using cran.Model.Dto;
using cran.Model.ViewModel;
using cran.Services;
using cran.tests.Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Xunit;


namespace cran.tests
{    
    

    public class BinaryServiceTest 
    {        

        [Fact]
        public async Task SaveBinaryTest()
        {

            TestingContext testingContext = new TestingContext();
            testingContext.AddAdminPrincipalMock();
            testingContext.AddLogServiceMock();            
            testingContext.AddRealDb();
            testingContext.AddUserService();

            IBinaryService service = testingContext.GetService<BinaryService>();

            string testWord = "Hello World";
            var bytes = System.Text.Encoding.UTF8.GetBytes(testWord);

            MemoryStream stream = new MemoryStream();
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            

            BinaryDto binaryDto = new BinaryDto
            {
                ContentDisposition = "ContentDisposition",
                ContentType = "ContentType",
                FileName = "FileName",
                Name = "Name",
                Length = 2334,
            };

            int id = await service.AddBinaryAsync(binaryDto);

            //Act
            await service.SaveAsync(id, stream);

            //Assert
            Stream streamToAssert = await service.GetBinaryAsync(id);
            StreamReader reader = new StreamReader(streamToAssert);
            string text = reader.ReadToEnd();
            Assert.Equal(testWord, text);

            //Cleanup
            await service.DeleteBinaryAsync(id);

        }   
    }
}
