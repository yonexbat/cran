using cran.Model.Dto;
using cran.Model.Entities;
using cran.Services;
using cran.Services.Exceptions;
using System;
using System.Collections.Generic;
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
            TestingContext context = new TestingContext();
            context.AddPrinicpalmock();
            context.AddBinaryServiceMock();
            context.AddInMemoryDb();
            context.AddMockLogService();

            ITagService tagService = context.GetService<TagService>();
            IList<int> tagIds = new List<int> { 1, 2, 3 };

            //Act
            IList<TagDto> tags = await tagService.GetTagsAsync(tagIds);

            //Assert
            Assert.Equal(3, tags.Count);            
        }

        [Fact]
        public async Task DeleteTagOk()
        {
            //Prepare
            TestingContext context = new TestingContext();
            context.AddAdminPrincipalMock();
            context.AddBinaryServiceMock();
            context.AddInMemoryDb();
            context.AddMockLogService();
            ITagService tagService = context.GetService<TagService>();

            //Act
            await  tagService.DeleteTagAsync(1);

            //Assert
            Exception ex = await Assert.ThrowsAsync<EntityNotFoundException>(async () =>
            {
                TagDto dto = await tagService.GetTagAsync(1);
            });            
        }

    }
}
