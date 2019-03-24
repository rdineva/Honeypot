using AutoMapper;
using Honeypot.Models;
using Honeypot.Services;
using Honeypot.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Honeypot.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<HoneypotUser> signInManager;
        private readonly IMapper mapper;
        private readonly HoneypotUsersService usersService;
        private readonly UserManager<HoneypotUser> userManager;

        public AccountController(SignInManager<HoneypotUser> signInManager, IMapper mapper,
            HoneypotUsersService usersService, UserManager<HoneypotUser> userManager)
        {
            this.signInManager = signInManager;
            this.mapper = mapper;
            this.usersService = usersService;
            this.userManager = userManager;
        }

        public IActionResult Register()
        {
            if (this.User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        public IActionResult Login()
        {
            if (this.User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Logout()
        {
            this.signInManager.SignOutAsync().Wait();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(viewModel);
            }

            var user = this.usersService.GetByUsername(viewModel.Username);

            if (user == null)
                return this.View();

            var result = signInManager.CheckPasswordSignInAsync(user, viewModel.Password, false).Result;

            if (result.Succeeded)
            {
                this.signInManager.SignInAsync(user, false).Wait();
                return RedirectToAction("Index", "Home");
            }

            return this.View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(viewModel);
            }

            var user = mapper.Map<HoneypotUser>(viewModel);
            var result = this.userManager.CreateAsync(user, viewModel.Password).Result;

            if (result.Succeeded)
            {
                this.signInManager.SignInAsync(user, false).Wait();

                IdentityResult identityResult = userManager.AddToRoleAsync(user, "User").Result;

                return this.RedirectToAction("Index", "Home");
            }

            return this.View();
        }

        [Authorize]
        public IActionResult Profile()
        {
            var currentUser = userManager.GetUserAsync(HttpContext.User).Result;
            var user = mapper.Map<ProfileViewModel>(currentUser);

            return this.View(user);
        }
    }
}