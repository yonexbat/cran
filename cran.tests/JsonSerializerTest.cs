using cran.Model.Dto;
using cran.Model.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace cran.tests
{
    public class JsonSerializerTest
    {
        [Fact]
        public async Task JsonSerializerTest_Ok()
        {
            QuestionDto dto = new QuestionDto();
            dto.Options.Add(new QuestionOptionDto());
            string serialized = System.Text.Json.JsonSerializer.Serialize<QuestionDto>(dto);
            Assert.Contains("Options", serialized);
        }

    }
}
