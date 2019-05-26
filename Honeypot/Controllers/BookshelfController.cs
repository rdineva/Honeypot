using Honeypot.Data;
using Honeypot.Models;
using Honeypot.ViewModels.Bookshelf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Honeypot.Services.Contracts;

namespace Honeypot.Controllers
{
    [Authorize]
    public class BookshelfController : BaseController
    {
        private readonly IUserService userService;
        private readonly IBookshelfService bookshelfService;

        public BookshelfController(HoneypotDbContext context, IUserService userService, IMapper mapper, IBookshelfService bookshelfService)
            : base(context, mapper)
        {
            this.userService = userService;
            this.bookshelfService = bookshelfService;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateBookshelfViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var createdBookshelf = this.OnPostCreateBookshelf(viewModel);
                return RedirectToAction("Details", new { id = createdBookshelf.Id });
            }

            return this.View(viewModel);
        }

        public IActionResult Details(int id)
        {
            var currentUser = this.userService.GetByUsername(this.User.Identity.Name);
            var bookshelfResult = this.bookshelfService.FindUserBookshelfById(id, currentUser.Id);
            if (bookshelfResult == null)
            {
                return this.RedirectToAction("/", "Home");
            }

            var bookshelf = this.mapper.Map<BookshelfDetailsViewModel>(bookshelfResult);
            return this.View(bookshelf);
        }

        public Bookshelf OnPostCreateBookshelf(CreateBookshelfViewModel viewModel)
        {
            var bookshelf = this.mapper.Map<Bookshelf>(viewModel);
            bookshelf.UserId = this.userService.GetByUsername(this.User.Identity.Name).Id;
            this.context.Bookshelves.Add(bookshelf);
            this.context.SaveChanges();
            return bookshelf;
        }
    }
}