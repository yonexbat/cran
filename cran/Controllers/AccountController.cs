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

namespace cran.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<ApplicationUser> _signInManager;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private readonly ILogger _logger;
        private readonly IUserProfileService _userProfileService;

        private static string Anonymous = "Anonymous";

        public AccountController(
            ILoggerFactory loggerFactory,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IUserProfileService userProfileService)
        {
            this._logger = loggerFactory.CreateLogger<AccountController>();
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._userProfileService = userProfileService;
        }

        public async Task<IActionResult> Login()
        {
            LoginViewModel vm = await GetLoginVm();
            return View(vm);
        }

        private async  Task<LoginViewModel> GetLoginVm()
        {
            LoginViewModel vm = new LoginViewModel();
            vm.ReturnUrl = Request.Query["ReturnUrl"];

            //External Login Providers
            IEnumerable<AuthenticationScheme> providers = await _signInManager.GetExternalAuthenticationSchemesAsync();
            vm.LoginProviders = providers.Select(x => new Model.Dto.LoginProviderDto()
            {
                DisplayName = x.DisplayName,
                Name = x.Name,
            }).ToList();

            //Anonymous
            vm.LoginProviders.Add(new Model.Dto.LoginProviderDto
            {
                DisplayName = "Anonymous",
                Name= Anonymous,
            });


            return vm;
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLogin(string provider, string returnUrl = null)
        {
            if(provider == Anonymous)
            {
                return await SignInAnonnymous();
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
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return View(nameof(Login));
            }
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);


            if (result.Succeeded)
            {
                _logger.LogInformation(5, "User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl });
            }
            if (result.IsLockedOut)
            {
                return View("Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                string email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = email });
            }
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

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                ApplicationUser user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                IdentityResult result = await _userManager.CreateAsync(user);

                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        await _userProfileService.CreateUserAsync(new UserInfoDto {Name = model.Email, IsAnonymous = false });                        
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
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