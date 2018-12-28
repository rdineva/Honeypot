using Honeypot.Data;
using Honeypot.Models;
using Honeypot.Services;
using Honeypot.ViewModels.Bookshelf;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Honeypot.Controllers
{
    public class BookshelfController : Controller
    {
        private readonly HoneypotUsersService usersService;
        private readonly HoneypotDbContext context;
        public BookshelfController(HoneypotUsersService usersService, HoneypotDbContext context)
        {
            this.usersService = usersService;
            this.context = context;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(viewModel);
            }

            if (string.IsNullOrWhiteSpace(viewModel.Title))
            {
                return this.View(viewModel);
            }

            var user = this.usersService.GetByUsername(this.User.Identity.Name);

            var bookshelf = new Bookshelf()
            {
                Title = viewModel.Title,
                OwnerId = user.Id,
                Owner = user,
            };

            this.context.Bookshelves.Add(bookshelf);
            this.context.SaveChanges();
            user.CustomBookshelves.Add(bookshelf);

            return RedirectToAction("Details", new { id = bookshelf.Id });
        }
    }
}