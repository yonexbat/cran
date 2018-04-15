using cran.Data;
using cran.Model.Dto;
using cran.Model.Entities;
using cran.Services;
using cran.tests.Infra;
using Microsoft.Extensions.Configuration;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Principal;
using System.Text;
using Xunit;

namespace cran.tests
{
    public class ExportTest 
    {
       


        [Fact]
        public async void TestExport()
        {

            //Prepare
            TestingContext context = new TestingContext();
            context.AddAdminPrincipalMock();
            context.AddBinaryServiceMock();
            context.AddInMemoryDb();
            context.AddMockLogService();
            context.AddGermanCultureServiceMock();
            context.AddQuestionService();      

            IExportService exportService = context.GetService<ExportService>();

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
                Assert.True(questions.Count > 0);
            }
            Assert.True(arch.Entries.Count > 0);
        }
    }
}
