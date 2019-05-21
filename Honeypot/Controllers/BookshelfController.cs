using Honeypot.Data;
using Honeypot.Models;
using Honeypot.Services;
using Honeypot.ViewModels.Bookshelf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AutoMapper;

namespace Honeypot.Controllers
{
    [Authorize]
    public class BookshelfController : BaseController
    {
        private readonly HoneypotUsersService usersService;

        public BookshelfController(HoneypotUsersService usersService, IMapper mapper, HoneypotDbContext context)
            : base(context, mapper)
        {
            this.usersService = usersService;
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

            var bookshelfUser = this.usersService.GetByUsername(this.User.Identity.Name);
            var bookshelfTitle = viewModel.Title;
            var bookshelf = new Bookshelf(bookshelfTitle, bookshelfUser.Id);

            this.context.Bookshelves.Add(bookshelf);
            bookshelfUser.CustomBookshelves.Add(bookshelf);
            this.context.SaveChanges();

            return RedirectToAction("Details", new { id = bookshelf.Id });
        }

        public IActionResult Details(int id)
        {
            var bookshelfResult = this.context.Bookshelves.FirstOrDefaultAsync(x => x.Id == id).Result;
            var user = this.usersService.GetByUsername(this.User.Identity.Name);

            if (bookshelfResult == null || bookshelfResult.UserId != user.Id)
            {
                return this.BadRequest("Bookshelf doesn't exist!");
            }

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