using cran.Model.Dto;
using cran.Services;
using cran.tests.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace cran.tests
{
    public class CacheServiceTest
    {

        [Fact]
        public  async Task GetEntryAsync()
        {
            TestingContext testingContext = new TestingContext();
            ICacheService cacheService = testingContext.GetService<CacheService>();

            Func<Task<TagDto>> getTagFunc = () => {
                TagDto result =  new TagDto
                {
                    Id = 3,
                    Name = "Test",
                };

                return Task.FromResult(result);
            };

            TagDto myTag = await cacheService.GetEntryAsync("MyKey", getTagFunc);

            Assert.Equal(3, myTag.Id);

            TagDto myTag2 = await cacheService.GetEntryAsync("MyKey", getTagFunc);

            Assert.True(ReferenceEquals(myTag, myTag2));
        }
    }
}
