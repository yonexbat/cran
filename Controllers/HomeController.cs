using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace cran.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger _logger;
        private readonly IFileProvider _fileProvider;
        
        public HomeController(ILoggerFactory loggerFactory, IFileProvider fileProvider)
        {
            _logger = loggerFactory.CreateLogger<HomeController>();
            _fileProvider = fileProvider;
        }        
        
        [Authorize]
        public IActionResult Index()
        {
            IFileInfo fileInfo = _fileProvider.GetFileInfo("wwwroot/jsclient/index.html");
            Stream stream = fileInfo.CreateReadStream();
            return File(stream, "text/html");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
