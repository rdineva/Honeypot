using Honeypot.Data;
using Honeypot.Models;
using Honeypot.Services;
using Honeypot.ViewModels.Bookshelf;
using Microsoft.AspNetCore.Authorization;
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
                UserId = user.Id,
                User = user,
            };

            this.context.Bookshelves.Add(bookshelf);
            user.CustomBookshelves.Add(bookshelf);
            this.context.SaveChanges();

            return RedirectToAction("Details", new { id = bookshelf.Id });
        }

        public IActionResult Details(int id)
        {
            var bookshelfResult = this.context.Bookshelves.FirstOrDefaultAsync(x => x.Id == id).Result;

            var user = this.usersService.GetByUsername(this.User.Identity.Name);

            if (bookshelfResult == null || bookshelfResult.UserId != user.Id)
                return this.BadRequest("Bookshelf doesn't exist!");

            var booksInBookshelf = this.context.BooksBookshelves.Where(x => x.BookshelfId == id).Select(x => x.BookId);

            var bookshelf = new DetailsViewModel()
            {
                Title = bookshelfResult.Title,
                OwnerId = bookshelfResult.UserId,
                OwnerNickname = bookshelfResult.User.UserName,
                Books = this.context.BooksBookshelves.Where(x => booksInBookshelf.Contains(x.BookId)).ToList()
                //Books = this.context.Books.Where(x => booksInBookshelf.Contains(x.Id)).ToList()
            };

            return this.View(bookshelf);
        }
    }
}