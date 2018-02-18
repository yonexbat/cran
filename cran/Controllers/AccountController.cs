using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using cran.Model.ViewModel;
using cran.Model.Entities;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using System;
using System.Security;
using cran.Services;
using cran.Model.Dto;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Localization;
using cran.Resources;

namespace cran.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger _logger;
        private readonly IUserProfileService _userProfileService;
        private readonly ICourseService _courseService;
        private readonly ITextService _textService;
        private readonly IStringLocalizer<SharedResources> _localizer;


        private static string Anonymous = "Anonymous";

        public AccountController(
            ILoggerFactory loggerFactory,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IUserProfileService userProfileService,
            ICourseService courseService,
            ITextService textService,
            IStringLocalizer<SharedResources> localizer)
        {
            this._logger = loggerFactory.CreateLogger<AccountController>();
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._userProfileService = userProfileService;
            this._courseService = courseService;
            this._textService = textService;
            this._localizer = localizer;
           
        }

        public async Task<IActionResult> Login()
        {          
            LoginViewModel vm = await GetLoginVm();           
            return View(vm);
        }

        private async Task<CourseDto> GetCourse(string input)
        {
            string pattern = @"/coursestarter/(?<id>\d+)$";
            Match match = Regex.Match(input, pattern);
            if(match.Success)
            {
                string idString = match.Groups["id"].Value;
                int id = int.Parse(idString);
                CourseDto dto = await _courseService.GetCourseAsync(id);
                return dto;
            }
            return null;                     
        }

        private async  Task<LoginViewModel> GetLoginVm()
        {
            LoginViewModel vm = new LoginViewModel();
            vm.ReturnUrl = Request.Query["ReturnUrl"];

            //External Login Providers
            IEnumerable<AuthenticationScheme> providers = await _signInManager.GetExternalAuthenticationSchemesAsync();
            vm.LoginProviders = providers.Select(x => new LoginProviderDto()
            {
                DisplayName = x.DisplayName,
                Name = x.Name,
                Tooltip = _localizer["LoginUsing", x.Name],
            }).ToList();

            //Anonymous
            vm.LoginProviders.Add(new LoginProviderDto
            {
                DisplayName = _localizer["anonymous"],
                Name= Anonymous,
                Tooltip = _localizer["LoginAnonymous"],
            });

            //Info Login
            string returnUrl = Request.Query["ReturnUrl"];
            vm.LoginInfoText = await _textService.GetTextAsync("LoginDefault");
            if (!string.IsNullOrWhiteSpace(returnUrl))
            {
                CourseDto course = await GetCourse(returnUrl);
                if (course != null)
                {
                    vm.LoginInfoText = await _textService.GetTextAsync("LoginStartCourse", course.Title);
                }
            }


            return vm;
        }



        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLogin(string provider, string returnUrl = null)
        {
            if(provider == Anonymous)
            {
                return await SignInAnonnymous(returnUrl);
            }

            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        private async Task<IActionResult> SignInAnonnymous(string returnUrl = null)
        {
            Guid guid = Guid.NewGuid();
            ApplicationUser user = new ApplicationUser { UserName = guid.ToString(),};
            IdentityResult identityResult = await _userManager.CreateAsync(user);

            if (identityResult.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: true);
                await _userProfileService.CreateUserAsync(new UserInfoDto { Name = user.UserName, IsAnonymous = true });
            }
            else
            {
                throw new SecurityException("Creating user failed");
            }
            return RedirectToLocal(returnUrl);
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {               
                return await ShowError($"Fehler: {remoteError}");                
            }
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return await ShowError($"Keine Infos zum User vorhanden");
            }

            // Sign in the user with this external login provider if the user already has a login.
            Microsoft.AspNetCore.Identity.SignInResult signInResult = 
                await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);

            if (signInResult.Succeeded)
            {
                _logger.LogInformation(5, "User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }
            if (signInResult.RequiresTwoFactor)
            {
                return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl });
            }
            if (signInResult.IsLockedOut)
            {
                return View("LockedOut");
            }
            else
            {
                return await CreateUser(returnUrl);                
            }
        }

        private async Task<IActionResult> ShowError(string error)
        {
            ModelState.AddModelError(string.Empty, error);
            LoginViewModel vm = await GetLoginVm();
            return View(nameof(Login), vm);
        }

        private async Task<IActionResult> CreateUser(string returnUrl)
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            string email = info.Principal.FindFirstValue(ClaimTypes.Email);
            ApplicationUser user = new ApplicationUser { UserName = email, Email = email };
            IdentityResult createUserResult = await _userManager.CreateAsync(user);

            if (createUserResult.Succeeded)
            {
                createUserResult = await _userManager.AddLoginAsync(user, info);
                if (createUserResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    await _userProfileService.CreateUserAsync(new UserInfoDto { Name = email, IsAnonymous = false });
                    return RedirectToLocal(returnUrl);
                }
            }
            AddErrors(createUserResult);
            return RedirectToLocal(returnUrl);
        }

        // GET: /Account/SendCode
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl = null, bool rememberMe = false)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            IList<string> userFactors = await _userManager.GetValidTwoFactorProvidersAsync(user);
            IList<SelectListItem> factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        [HttpGet]
        public async Task<IActionResult> DoLogout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
            
    }
}