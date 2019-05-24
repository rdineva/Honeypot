using Honeypot.Data;
using Honeypot.Models;
using Honeypot.Services;
using Honeypot.ViewModels.Bookshelf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Honeypot.Controllers
{
    [Authorize]
    public class BookshelfController : BaseController
    {
        private readonly UserService usersService;

        public BookshelfController(UserService usersService, IMapper mapper, HoneypotDbContext context)
            : base(context, mapper)
        {
            this.usersService = usersService;
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
            var bookshelfResult = this.context.Bookshelves.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id).Result;

            var currentUser = this.usersService.GetByUsername(this.User.Identity.Name);
            if (bookshelfResult == null || bookshelfResult.UserId != currentUser.Id)
            {
                return this.BadRequest("Bookshelf doesn't exist!");
            }

            var bookshelf = this.mapper.Map<BookshelfDetailsViewModel>(bookshelfResult);
            return this.View(bookshelf);
        }

        public Bookshelf OnPostCreateBookshelf(CreateBookshelfViewModel viewModel)
        {
            var bookshelf = this.mapper.Map<Bookshelf>(viewModel);
            bookshelf.UserId = this.usersService.GetByUsername(this.User.Identity.Name).Id;
            this.context.Bookshelves.Add(bookshelf);
            this.context.SaveChanges();
            return bookshelf;
        }
    }
}