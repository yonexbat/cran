using cran.Data;
using cran.Model.Dto;
using cran.Model.Entities;
using cran.Services;
using cran.Services.Exceptions;
using cran.tests.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using Xunit;

namespace cran.tests
{
    public class TagServiceTest
    {

        private void SetupContext(TestingContext context)
        {
            context.AddPrincipalMock();
            context.AddBinaryServiceMock();
            context.AddInMemoryDb();
            context.AddLogServiceMock();
            context.AddCacheService();
        }

        [Fact]
        public async Task AddTag()
        {
            //Prepare
            TestingContext context = new TestingContext();
            SetupContext(context);
            

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
            SetupContext(testingContext);

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
            SetupContext(testingContext);
            testingContext.AddAdminPrincipalMock();      
            
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

        [Fact]
        public async Task DeleteTagNoRights()
        {
            TestingContext testingContext = new TestingContext();
            SetupContext(testingContext);

            ITagService tagService = testingContext.GetService<TagService>();
            ApplicationDbContext dbContext = testingContext.GetSimple<ApplicationDbContext>();

            int firstTagId = dbContext.Tags.First().Id;

            //Act and Assert
            Exception ex = await Assert.ThrowsAsync<SecurityException>(async () =>
            {
                await tagService.DeleteTagAsync(firstTagId);
            });
            
        }

        [Fact]
        public async Task FindTagsAsync()
        {
            TestingContext testingContext = new TestingContext();
            SetupContext(testingContext);
            ITagService tagService = testingContext.GetService<TagService>();

            IList<TagDto> result = await tagService.FindTagsAsync("e");

            Assert.Equal(4, result.Count);
            Assert.All(result, tagDto => Assert.True(tagDto.IdTagType == (int)TagType.Standard));
        }

        [Theory]
        [InlineData(SpecialTag.Deprecated)]
        public async Task GetSpecialTag(SpecialTag specialTag)
        {
            TestingContext testingContext = new TestingContext();
            SetupContext(testingContext);
            ITagService tagService = testingContext.GetService<TagService>();
            TagDto tagDto = await tagService.GetSpecialTagAsync(specialTag);
            Assert.NotNull(tagDto);
        }

    }
}
