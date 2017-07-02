using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

 
namespace cran.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<string> _signInManager; 

        public AccountController(SignInManager<string>  signinManager)
        {
            this._signInManager = signinManager;
        }

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

        public IActionResult SignOut()
        {            
        
            return View(nameof(Login));
        }
    }
}