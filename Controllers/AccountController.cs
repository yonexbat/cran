using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;

 
namespace cran.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
 
        public IActionResult External(string provider)
        {
            var authProperties = new AuthenticationProperties
            {
                RedirectUri = "/home/index"
            };
 
            return new ChallengeResult(provider, authProperties);
        }
    }
}