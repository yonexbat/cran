using cran.Data;
using cran.Model.Dto;
using cran.Model.Entities;
using cran.Services;
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
    public class ExportTest : Base
    {
       


        protected override void SetUpDependencies(IDictionary<Type, object> dependencyMap)
        {
            base.SetUpDependencies(dependencyMap);
            dependencyMap[typeof(ICommentsService)] = GetServiceInMemoryDb<CommentsService>();
            dependencyMap[typeof(ITextService)] = GetServiceInMemoryDb<TextService>();
            dependencyMap[typeof(IQuestionService)] = GetServiceInMemoryDb<QuestionService>();
            dependencyMap[typeof(IBinaryService)] = CreateBinaryServiceMock();
            dependencyMap[typeof(IPrincipal)] = GetPricipalAdminMock();
        }


        [Fact]
        public async void TestExport()
        {
            //Prepare
            IExportService exportService = GetServiceInMemoryDb<ExportService>();

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
