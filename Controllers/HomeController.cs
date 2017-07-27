using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace cran.Controllers
{
    public class HomeController : Controller
    {

        private readonly ILogger _logger;
        
        public HomeController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HomeController>();
        }        
        
        [Authorize]
        public IActionResult Index()
        {
            return Redirect("/jsclient/index.html");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
