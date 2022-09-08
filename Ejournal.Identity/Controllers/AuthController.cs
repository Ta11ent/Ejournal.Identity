using Ejournal.Identity.Models;
using IdentityServer4.Services;
using MailService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ejournal.Identity.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IIdentityServerInteractionService _interactionService;
        private readonly IMailService _mailService;

        public AuthController(SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager, IIdentityServerInteractionService interactionService, IMailService mailService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _interactionService = interactionService;
            _mailService = mailService;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            var viewModel = new Login
            {
                ReturnUrl = returnUrl
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var user = await _userManager.FindByEmailAsync(viewModel.Email);
            if (user == null)
            {
                viewModel.Invalid = true;
                ModelState.AddModelError(nameof(viewModel.Invalid), "User not found");
                return View(viewModel);
            }

            var succeeded = await _userManager.CheckPasswordAsync(user, viewModel.Password);
            if (succeeded && !user.AccountConfirmed)
            {
              
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action(nameof(ConfirmAccount), "Auth",
                    new 
                    { 
                        token, 
                        email = user.Email, 
                        returnUrl = viewModel.ReturnUrl 
                    },
                    Request.Scheme);
                await _mailService.SendEmailTemplateAsync("Confirm Account", user.Email, confirmationLink);
                return View(nameof(Info));
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName,
                viewModel.Password,  viewModel.Remeber, false);
            if (result.Succeeded)
            {
                return Redirect(viewModel.ReturnUrl);
            }

            viewModel.Invalid = true;
            ModelState.AddModelError(nameof(viewModel.Invalid), "Login error");
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Logout(string LogoutId)
        {
            await _signInManager.SignOutAsync();
            var logoutRequeest = await _interactionService.GetLogoutContextAsync(LogoutId);
            return Redirect(logoutRequeest.PostLogoutRedirectUri);
        }

        [HttpGet]
        public IActionResult Info()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ConfirmAccount(string token, string email, string returnUrl)
        {
            var user = _userManager.FindByEmailAsync(email);
            if (user.Result.AccountConfirmed)
                return View(nameof(Error));

            var viewModel = new Confirm
            {
                Token = token,
                Email = email,
                ReturnUrl = returnUrl
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmAccount(Confirm viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);

            var user = await _userManager.FindByEmailAsync(viewModel.Email);
            if (user == null)
                return View(nameof(Error));

            await _userManager.ConfirmEmailAsync(user, viewModel.Token);
            await _userManager.RemovePasswordAsync(user);
            await _userManager.AddPasswordAsync(user, viewModel.Password);
            user.AccountConfirmed = true;
            await _userManager.UpdateAsync(user);
            return View(nameof(Login), new Login { ReturnUrl = viewModel.ReturnUrl} );
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }
    }        
}
