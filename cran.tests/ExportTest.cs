using cran.Data;
using cran.Model.Dto;
using cran.Model.Entities;
using cran.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using Xunit;

namespace cran.tests
{
    public class ExportTest : Base
    {

        protected IExportService CreateExportService()
        {
            //Prepare                       
            IConfiguration config = GetConfiguration();
            ApplicationDbContext context = CreateInMemoryDbContext();

            var questionServiceTestingObject = new TestingObject<QuestionService>();
            questionServiceTestingObject.AddDependency(context);
            questionServiceTestingObject.AddDependency(new Mock<IDbLogService>(MockBehavior.Loose));
            questionServiceTestingObject.AddDependency(GetPricipalAdminMock());
            questionServiceTestingObject.AddDependency(new Mock<ICommentsService>(MockBehavior.Loose));
            questionServiceTestingObject.AddDependency(new Mock<ITextService>(MockBehavior.Loose));
            IQuestionService questionService = questionServiceTestingObject.GetResolvedTestingObject();

            IBinaryService binaryService = CreateBinaryServiceMock();

            var exportServiceTestingObject = new TestingObject<ExportService>();
            exportServiceTestingObject.AddDependency(context);
            exportServiceTestingObject.AddDependency(new Mock<IDbLogService>(MockBehavior.Loose));
            exportServiceTestingObject.AddDependency(GetPricipalAdminMock());
            exportServiceTestingObject.AddDependency(questionService);
            exportServiceTestingObject.AddDependency(binaryService);
            return exportServiceTestingObject.GetResolvedTestingObject();
        }




        

        [Fact]
        public async void TestExport()
        {
            //Prepare
            IExportService exportService = CreateExportService();

            //Act
            Stream stream = await exportService.Export();

            //Assert
            ZipArchive arch = new ZipArchive(stream);
            ZipArchiveEntry entry = arch.GetEntry("questions.json");
            Stream zipEntryStream = entry.Open();
            using (var reader = new StreamReader(zipEntryStream, Encoding.UTF8))
            {
                string value = reader.ReadToEnd();
                IList<QuestionDto> questions = JsonConvert.DeserializeObject<IList<QuestionDto>>(value);
                Assert.Equal(100, questions.Count);
            }
            Assert.Equal(301, arch.Entries.Count);
        }
    }
}
