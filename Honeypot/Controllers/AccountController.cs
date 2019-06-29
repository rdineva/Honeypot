using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Honeypot.Data;
using Honeypot.Models;
using Honeypot.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace Honeypot.Controllers
{
    public class AccountController : BaseController
    {
        private readonly SignInManager<HoneypotUser> signInManager;
        private readonly UserManager<HoneypotUser> userManager;

        public AccountController(HoneypotDbContext context, IMapper mapper, SignInManager<HoneypotUser> signInManager, UserManager<HoneypotUser> userManager)
            : base(context, mapper)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        public IActionResult Register()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public IActionResult Login()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Logout()
        {
            this.signInManager.SignOutAsync().Wait();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var logInResult = this.OnPostLoginAsync(viewModel).GetAwaiter().GetResult();
                if (logInResult.Succeeded)
                {
                    return RedirectToAction("/", "Home");
                }

                ModelState.AddModelError("Username", ControllerConstants.IncorrectLogin);
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var registerResult = this.OnPostRegisterAsync(viewModel).GetAwaiter().GetResult();
                if (registerResult.Succeeded)
                {
                    return this.RedirectToAction("/", "Home");
                }
            }

            return this.View(viewModel);
        }

        [Authorize]
        public IActionResult Profile()
        {
            var currentUser = userManager.GetUserAsync(HttpContext.User).Result;
            var userProfileViewModel = mapper.Map<ProfileViewModel>(currentUser);
            return this.View(userProfileViewModel);
        }

        private async Task<IdentityResult> OnPostRegisterAsync(RegisterViewModel viewModel)
        {
            var user = mapper.Map<RegisterViewModel, HoneypotUser>(viewModel);
            var registerResult = await this.userManager.CreateAsync(user, viewModel.Password);
            if (registerResult.Succeeded)
            {
                await this.AddRoleToAccountAsync(user);
                await this.signInManager.SignInAsync(user, isPersistent: false);
            }

            return registerResult;
        }

        private async Task AddRoleToAccountAsync(HoneypotUser user)
        {
            if (this.context.Users.Count() == 1)
            {
                await this.userManager.AddToRoleAsync(user, Role.Admin);
            }
            else
            {
                await this.userManager.AddToRoleAsync(user, Role.User);
            }
        }

        private async Task<SignInResult> OnPostLoginAsync(LoginViewModel viewModel)
        {
            var loginResult = await this.signInManager.PasswordSignInAsync(viewModel.Username, viewModel.Password, isPersistent: false, lockoutOnFailure: false);
            return loginResult;
        }
    }
}