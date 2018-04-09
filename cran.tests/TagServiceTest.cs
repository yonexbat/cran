using cran.Data;
using cran.Model.Dto;
using cran.Model.Entities;
using cran.Services;
using cran.Services.Exceptions;
using cran.tests.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace cran.tests
{
    public class TagServiceTest
    {
        [Fact]
        public async Task AddTag()
        {
            //Prepare
            TestingContext context = new TestingContext();
            context.AddPrinicpalmock();
            context.AddBinaryServiceMock();
            context.AddInMemoryDb();
            context.AddMockLogService();

            ITagService tagService = context.GetService<TagService>();

            TagDto tag = new TagDto()
            {
                Name = "TestTag",
                ShortDescDe = "ShortDescDe",
                ShortDescEn = "ShortDescEn",
            };

            //Act
            int id = await tagService.InsertTagAsync(tag);
            TagDto tagDto = await tagService.GetTagAsync(id);

            //Assert
            Assert.True(id > 0);
            Assert.Equal(TagType.Standard, (TagType) tagDto.IdTagType);

        }

        [Fact]
        public async Task GetTags()
        {
            //Prepare
            TestingContext testingContext = new TestingContext();
            testingContext.AddPrinicpalmock();
            testingContext.AddBinaryServiceMock();
            testingContext.AddInMemoryDb();
            testingContext.AddMockLogService();

            ApplicationDbContext dbContext = testingContext.GetSimple<ApplicationDbContext>();

            ITagService tagService = testingContext.GetService<TagService>();
            IList<int> tagIds = dbContext.Tags.Select(x => x.Id).Take(3).ToList();

            //Act
            IList<TagDto> tags = await tagService.GetTagsAsync(tagIds);

            //Assert
            Assert.Equal(3, tags.Count);            
        }

        [Fact]
        public async Task DeleteTagOk()
        {
            //Prepare
            TestingContext testingContext = new TestingContext();
            testingContext.AddAdminPrincipalMock();
            testingContext.AddBinaryServiceMock();
            testingContext.AddInMemoryDb();
            testingContext.AddMockLogService();
            ITagService tagService = testingContext.GetService<TagService>();
            ApplicationDbContext dbContext = testingContext.GetSimple<ApplicationDbContext>();

            int firstTagId = dbContext.Tags.First().Id;

            //Act
            await  tagService.DeleteTagAsync(firstTagId);

            //Assert
            Exception ex = await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
            {
                TagDto dto = await tagService.GetTagAsync(firstTagId);
            });            
        }

    }
}
