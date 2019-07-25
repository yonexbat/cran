using cran.Data;
using cran.Model.Dto;
using cran.Model.Entities;
using cran.Services;
using cran.tests.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace cran.tests
{
    public class TextServiceTest
    {
        [Fact]
        public async Task GetTextDeTest()
        {
            TestingContext testingContext = new TestingContext();
            testingContext.AddAdminPrincipalMock();
            testingContext.AddInMemoryDb();
            testingContext.AddLogServiceMock();
            testingContext.AddGermanCultureServiceMock();
            
            ApplicationDbContext dbContext = testingContext.GetSimple<ApplicationDbContext>();
            dbContext.Texts.Add(new Text()
            {
                Key = "TestKey",
                ContentDe = "ContentDe  {0} {1} {2}",
                ContentEn = "ContentEn {0} {1} {2}",
            });
            await dbContext.SaveChangesAsync();

            ITextService textService = testingContext.GetService<TextService>();

            //Act            
            string text = await textService.GetTextAsync("TestKey", "pl1", "pl2", "pl3");

            //Assert
            Assert.Equal("ContentDe  pl1 pl2 pl3", text);         
            
        }

        [Fact]
        public async Task GetTextEnTest()
        {
            TestingContext testingContext = new TestingContext();
            testingContext.AddAdminPrincipalMock();
            testingContext.AddInMemoryDb();
            testingContext.AddLogServiceMock();
            testingContext.AddEnglishCultureServiceMock();

            ApplicationDbContext dbContext = testingContext.GetSimple<ApplicationDbContext>();
            dbContext.Texts.Add(new Text()
            {
                Key = "TestKey",
                ContentDe = "ContentDe  {0} {1} {2}",
                ContentEn = "ContentEn {0} {1} {2}",
            });
            await dbContext.SaveChangesAsync();

            ITextService textService = testingContext.GetService<TextService>();

            //Act            
            string text = await textService.GetTextAsync("TestKey", "pl1", "pl2", "pl3");

            //Assert
            Assert.Equal("ContentEn pl1 pl2 pl3", text);

        }

        [Fact]
        public async void TestPaging()
        {
            TestingContext testingContext = new TestingContext();
            testingContext.AddAdminPrincipalMock();
            testingContext.AddInMemoryDb();
            testingContext.AddLogServiceMock();
            testingContext.AddEnglishCultureServiceMock();

            ApplicationDbContext dbContext = testingContext.GetSimple<ApplicationDbContext>();
            for(int i=0; i<13; i++)
            {
                dbContext.Texts.Add(new Text()
                {
                    Key = $"TestKey{i}",
                    ContentDe = $"ContentDe",
                    ContentEn = $"ContentEn",
                });
            }
           
            await dbContext.SaveChangesAsync();
            ITextService textService = testingContext.GetService<TextService>();

            //Act
            SearchTextDto dto = new SearchTextDto();
            dto.Page = 0;
            PagedResultDto<TextDto> res = await textService.GetTextsAsync(dto);

            //Assert
            Assert.Equal(13, res.Count);
            Assert.Equal(5, res.Pagesize);
            Assert.Equal(5, res.Data.Count);
            Assert.Equal(3, res.Numpages);

            //Act
            dto = new SearchTextDto();
            dto.Page = 2;
            res = await textService.GetTextsAsync(dto);

            //Assert
            Assert.Equal(13, res.Count);
            Assert.Equal(5, res.Pagesize);
            Assert.Equal(3, res.Data.Count);
            Assert.Equal(3, res.Numpages);
        }


    }
}
