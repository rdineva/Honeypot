using Honeypot.Data;
using Honeypot.Models;
using Honeypot.Services;
using Honeypot.ViewModels.Bookshelf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Honeypot.Controllers
{
    [Authorize]
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

        public IActionResult Details(int id)
        {
            var bookshelfResult = this.context.Bookshelves.FirstOrDefaultAsync(x => x.Id == id).Result;

            var user = this.usersService.GetByUsername(this.User.Identity.Name);

            if (bookshelfResult == null || bookshelfResult.OwnerId != user.Id)
                return this.BadRequest("Bookshelf doesn't exist!");

            var bookshelf = new DetailsViewModel()
            {
                Title = bookshelfResult.Title,
                OwnerId = bookshelfResult.OwnerId,
                OwnerNickname = bookshelfResult.Owner.UserName,
                Books = bookshelfResult.Books
            };

            return this.View(bookshelf);
        }
    }
}