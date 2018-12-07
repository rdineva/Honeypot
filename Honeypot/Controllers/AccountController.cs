using AutoMapper;
using Honeypot.Models;
using Honeypot.Services;
using Honeypot.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Honeypot.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<HoneypotUser> signInManager;
        private readonly IMapper mapper;
        private readonly HoneypotUsersService usersService;

        public AccountController(SignInManager<HoneypotUser> signInManager, IMapper mapper, HoneypotUsersService usersService)
        {
            this.signInManager = signInManager;
            this.mapper = mapper;
            this.usersService = usersService;
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel viewModel)
        {
            var user = this.usersService.GetByUsername(viewModel.Username);
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
            var user = mapper.Map<HoneypotUser>(viewModel);

            var result = this.signInManager.UserManager.CreateAsync(user, viewModel.Password).Result;

            if (result.Succeeded)
            {
                this.signInManager.SignInAsync(user, false).Wait();
                return this.RedirectToAction("Index", "Home");
            }

            return this.View();
        }
    }
}
