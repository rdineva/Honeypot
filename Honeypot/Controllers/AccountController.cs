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
                //TODO: add error when unsuccessfull login
                if (logInResult.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return this.View(viewModel);

        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var registerResult = this.OnPostRegisterAsync(viewModel);
                if (registerResult.Succeeded)
                {
                    return this.RedirectToAction("Index", "Home");
                }
                //TODO: add error when unsuccessful register
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

        private IdentityResult OnPostRegisterAsync(RegisterViewModel viewModel)
        {
            var user = mapper.Map<RegisterViewModel, HoneypotUser>(viewModel);
            var registerResult = this.userManager.CreateAsync(user, viewModel.Password).GetAwaiter().GetResult();

            if (registerResult.Succeeded)
            {
                if (this.context.Users.Count() == 1)
                {
                    this.userManager.AddToRoleAsync(user, "Admin").GetAwaiter().GetResult();
                }
                else
                {
                    this.userManager.AddToRoleAsync(user, "User").GetAwaiter().GetResult();
                }

                this.signInManager.SignInAsync(user, isPersistent: false).GetAwaiter().GetResult();
            }

            return registerResult;
        }

        private async Task<SignInResult> OnPostLoginAsync(LoginViewModel viewModel)
        {
            var loginResult = await this.signInManager.PasswordSignInAsync(viewModel.Username, viewModel.Password, isPersistent: false, lockoutOnFailure: false);

            return loginResult;
        }
    }
}