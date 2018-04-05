using cran.Model.Dto;
using cran.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace cran.tests
{
    public class QuestionServiceTest : Base
    {
        protected override void SetUpDependencies(IDictionary<Type, object> dependencyMap)
        {
            base.SetUpDependencies(dependencyMap);

            dependencyMap[typeof(ICommentsService)] = GetServiceInMemoryDb<CommentsService>();
            dependencyMap[typeof(ITextService)] = GetServiceInMemoryDb<TextService>();
            dependencyMap[typeof(IQuestionService)] = GetServiceInMemoryDb<QuestionService>();           
        }


        [Fact]
        public async Task TestImage()
        {
            IQuestionService questionService = GetServiceInMemoryDb<QuestionService>();


            //Add Q
            QuestionDto qdto = new QuestionDto()
            {
                Title = "Bla",
                Explanation = "bla",
                Language = "De",
            };
            int newId = await questionService.InsertQuestionAsync(qdto);

            //Add Binary
            IBinaryService binaryService = GetService<BinaryService>();
            int id = await binaryService.AddBinaryAsync(new BinaryDto
            {
                ContentDisposition = "ContentDisposition",
                ContentType = "ContentType",
                FileName = "FileName",
                Name = "Name",
                Length = 2334,
            });


            ImageDto imageDto = new ImageDto()
            {
                IdBinary = id,
                Full = false,
                Height = 124,
                Width = 64,
            };

            imageDto = await questionService.AddImageAsync(imageDto);
            QuestionDto questionDto = await questionService.GetQuestionAsync(newId);
            questionDto.Images.Add(imageDto);

            await questionService.UpdateQuestionAsync(questionDto);

            questionDto = await questionService.GetQuestionAsync(newId);

            Assert.True(questionDto.Images.Count == 1);
        }
    }
}
