using System.Linq;
using Honeypot.Data;
using Honeypot.Models;
using Honeypot.ViewModels.Bookshelf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Honeypot.Services.Contracts;
using Honeypot.Constants;
using Honeypot.Models.MappingModels;

namespace Honeypot.Controllers
{
    [Authorize]
    public class BookshelfController : BaseController
    {
        private readonly IAccountService accountService;
        private readonly IBookshelfService bookshelfService;
        private readonly IBookService bookService;

        public BookshelfController(HoneypotDbContext context, IAccountService accountService, IMapper mapper, IBookService bookService, IBookshelfService bookshelfService)
            : base(context, mapper)
        {
            this.accountService = accountService;
            this.bookService = bookService;
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
            var currentUser = this.accountService.GetByUsername(this.User.Identity.Name);
            var bookshelfResult = this.bookshelfService.GetUserBookshelfById(id, currentUser.Id);
            if (bookshelfResult == null)
            {
                return this.RedirectToAction("/", "Home");
            }

            var bookshelf = this.mapper.Map<BookshelfDetailsViewModel>(bookshelfResult);
            return this.View(bookshelf);
        }

        [HttpPost]
        public IActionResult AddToBookshelf(int bookshelfId, int bookId)
        {
            var user = this.accountService.GetByUsername(this.User.Identity.Name);
            var bookResult = this.bookService.GetBookById(bookId);
            ValidateAddToBookshelf(bookshelfId, bookId, bookResult, user);

            if (ModelState.IsValid)
            {
                this.OnPostAddToBookshelf(bookshelfId, bookResult, user);
                return RedirectToAction("Details", "Bookshelf", new { id = bookshelfId });
            }

            return this.RedirectToAction("/", "Home");
        }

        public IActionResult RemoveFromBookshelf(int bookshelfId, int bookId)
        {
            var user = this.accountService.GetByUsername(this.User.Identity.Name);
            var bookshelf = this.bookshelfService.GetUserBookshelfById(bookshelfId, user.Id);
            var bookInBookshelf = bookshelf.Books.FirstOrDefault(x => x.BookId == bookId);
            if (bookshelf != null && bookInBookshelf != null)
            {
                this.context.Remove(bookInBookshelf);
                this.context.SaveChanges();
            }

            return this.RedirectToAction("Details", "Bookshelf", new { id = bookshelf.Id });
        }

        private void OnPostAddToBookshelf(int bookshelfId, Book book, HoneypotUser user)
        {
            var bookBookshelf = new BookBookshelf()
            {
                BookId = book.Id,
                Book = book,
                BookshelfId = bookshelfId
            };

            user.CustomBookshelves.First(x => x.Id == bookshelfId).Books.Add(bookBookshelf);
            this.context.SaveChanges();
        }

        public IActionResult MyBookshelves()
        {
            var user = this.accountService.GetByUsername(this.User.Identity.Name);
            var userBokshelves = this.bookshelfService.GetUsersBookshelves(user.UserName);
            var bookshelves = new MyBookshelvesViewModel()
            {
                Bookshelves = userBokshelves
            };

            return this.View(bookshelves);
        }

        public IActionResult Delete(int id)
        {
            var user = this.accountService.GetByUsername(this.User.Identity.Name);
            var bookshelf = this.bookshelfService.GetUserBookshelfById(id, user.Id);
            if (bookshelf != null)
            {
                this.context.Remove(bookshelf);
                this.context.SaveChanges();
            }

            return this.RedirectToAction("MyBookshelves", "Bookshelf");
        }

        private Bookshelf OnPostCreateBookshelf(CreateBookshelfViewModel viewModel)
        {
            var bookshelf = this.mapper.Map<Bookshelf>(viewModel);
            bookshelf.UserId = this.accountService.GetByUsername(this.User.Identity.Name).Id;
            this.context.Bookshelves.Add(bookshelf);
            this.context.SaveChanges();
            return bookshelf;
        }

        //INPUT DATA VALIDATION METHODS
        private void ValidateAddToBookshelf(int bookshelfId, int bookId, Book bookResult, HoneypotUser user)
        {
            ValidateBookExists(bookResult);
            ValidateUserBookshelfIdExists(bookshelfId, user);
            ValidateBookIsntInBookshelf(bookId, bookshelfId);
        }

        private void ValidateUserBookshelfIdExists(int bookshelfId, HoneypotUser user)
        {
            if (this.bookshelfService.GetUserBookshelfById(bookshelfId, user.Id) == null)
            {
                var errorMessage = string.Format(GeneralConstants.DoesntExist, typeof(Bookshelf).Name);
                ModelState.AddModelError("Bookshelf", errorMessage);
            }
        }

        private void ValidateBookExists(Book book)
        {
            if (book == null)
            {
                var errorMessage = string.Format(GeneralConstants.DoesntExist, typeof(Book).Name);
                ModelState.AddModelError("Book", errorMessage);
            }
        }

        private void ValidateBookIsntInBookshelf(int bookId, int bookshelfId)
        {
            if (this.bookshelfService.IsBookInBookshelf(bookId, bookshelfId))
            {
                ModelState.AddModelError("Book", "Book is already on that bookshelf!");
            }
        }
    }
}