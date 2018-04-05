using cran.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace cran.tests
{
    public class InMemoryEntityTest : Base
    {
        [Fact]
        public async Task TestCourse()
        {
            var dbContext = CreateInMemoryDbContext();

            CourseInstance courseInstance = new CourseInstance();
            dbContext.CourseInstances.Add(courseInstance);

            await dbContext.SaveChangesAsync();

            int id = courseInstance.Id;
            Assert.True(id > 0);
        }

        [Fact]
        public async Task TestCranUser()
        {
            var dbContext = CreateInMemoryDbContext();

            CranUser cranUser = new CranUser();
            dbContext.CranUsers.Add(cranUser);

            await dbContext.SaveChangesAsync();

            int id = cranUser.Id;
            Assert.True(id > 0);
        }
    }
}
