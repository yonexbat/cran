using cran.Model.Dto;
using cran.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace cran.tests
{
    public class QuestionServiceTest 
    {

        [Fact]
        public async Task TestImage()
        {
            TestingContext context = new TestingContext();
            context.AddPrinicpalmock();
            context.AddInMemoryDb();
            context.AddMockLogService();
            context.DependencyMap[typeof(IBinaryService)] = context.GetService<BinaryService>();
            context.DependencyMap[typeof(ITextService)] = context.GetService<TextService>();
            context.DependencyMap[typeof(ICommentsService)] = context.GetService<CommentsService>();
          


            IQuestionService questionService = context.GetService<QuestionService>();


            //Add Q
            QuestionDto qdto = new QuestionDto()
            {
                Title = "Bla",
                Explanation = "bla",
                Language = "De",
            };
            int newId = await questionService.InsertQuestionAsync(qdto);

            //Add Binary
            IBinaryService binaryService = context.GetSimple<IBinaryService>();
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
