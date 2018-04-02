using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security;
using System.Security.Principal;
using System.Threading.Tasks;
using cran.Data;
using cran.Model.Dto;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace cran.Services
{
    public class ExportService : CraniumService, IExportService
    {
        private IQuestionService _questionService;
        private IBinaryService _binaryService;

        public ExportService(ApplicationDbContext context, 
            IDbLogService dbLogService,
            IPrincipal principal,
            IQuestionService questionService,
            IBinaryService binaryService) : base(context, dbLogService, principal)
        {
            _questionService = questionService;
            _binaryService = binaryService;
        }

        public async Task<Stream> Export()
        {
            //Security Check
            if(!_currentPrincipal.IsInRole(Roles.Admin))
            {
                throw new SecurityException($"Admin rights required");
            }

           

            MemoryStream stream = new MemoryStream();

            using (ZipArchive zip = new ZipArchive(stream, ZipArchiveMode.Create, true))
            {

                ZipArchiveEntry entry = zip.CreateEntry("questions.json");
                using (Stream entryStream = entry.Open())
                {
                    await ExportQuestions(entryStream);
                }
                await ExportBinaries(zip);
            }

            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        private async Task ExportBinaries(ZipArchive zip)
        {
            IQueryable<int> questionList = GetQuestionIds();
            var result = await _context.RelQuestionImages
                .Where(x => questionList.Contains(x.Question.Id))
                .Select(x => new
                {

                    BinaryId = x.Image.Binary.Id,
                    FileName = x.Image.Binary.FileName,
                }).ToListAsync();
            foreach(var item in result)
            {
                string fileName = $"{item.BinaryId}_{item.FileName}";
                ZipArchiveEntry entry = zip.CreateEntry(fileName);
                using(Stream entryStream  = entry.Open())
                {
                    Stream fileStream = await _binaryService.GetBinaryAsync(item.BinaryId);
                    await fileStream.CopyToAsync(entryStream);
                }
            }
            
        }

        private async Task ExportQuestions(Stream stream)
        {
            IList<QuestionDto> questions = await GetQuestions();
            string text = JsonConvert.SerializeObject(questions);
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(text);
            stream.Write(bytes, 0, bytes.Length);
            
        }

        private async Task<IList<QuestionDto>> GetQuestions()
        {
            IList<int> questionList = await GetQuestionIds().ToListAsync();
            IList<QuestionDto> result = new List<QuestionDto>();
            foreach(int qid in questionList)
            {
                var q = await _questionService.GetQuestionAsync(qid);
                result.Add(q);
            }
            return result;
        }

        private IQueryable<int> GetQuestionIds()
        {
            return _context.Questions
                .Where(x => x.Status == Model.Entities.QuestionStatus.Released || x.Status == Model.Entities.QuestionStatus.Created)
                .Select(x => x.Id);

        }
        
    }
}
