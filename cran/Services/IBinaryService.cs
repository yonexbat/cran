using cran.Model.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace cran.Services
{
    public interface IBinaryService
    {
        Task<IList<FileDto>> UploadFilesAsync(IList<IFormFile> files);

        Task<Stream> GetBinaryAsync(int id);

        Task<FileDto> GetFileInfoAsync(int id);

        Task SaveAsync(int id, Stream input);
    }
}
