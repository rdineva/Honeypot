using AutoMapper;
using Honeypot.Data;
using Honeypot.Models;
using Honeypot.Models.MappingModels;
using Honeypot.ViewModels.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Honeypot.Services.Contracts;

namespace Honeypot.Controllers
{
    [Authorize]
    public class BookController : BaseController
    {
        private readonly IUserService usersService;
        private readonly IAuthorService authorService;
        private readonly IBookshelfService bookshelfService;
        private readonly IBookService bookService;

        public BookController(HoneypotDbContext context, IMapper mapper, IUserService usersService, IAuthorService authorService, IBookshelfService bookshelfService, IBookService bookService)
            : base(context, mapper)
        {
            this.usersService = usersService;
            this.authorService = authorService;
            this.bookshelfService = bookshelfService;
            this.bookService = bookService;
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var bookResult = this.bookService.GeBookById(id);
            if (bookResult == null)
            {
                return this.RedirectToAction("/", "Home");
            }

            var book = this.mapper.Map<BookDetailsViewModel>(bookResult);
            return View(book);
        }

        [Authorize(Roles = Role.Admin)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public IActionResult Create(CreateBookViewModel viewModel)
        {
            ValidateAuthorNamesExist(viewModel);
            ValidateBookTitleDoesntExist(viewModel);

            if (ModelState.IsValid)
            {
                var bookResult = OnPostCreateBook(viewModel);
                return RedirectToAction("Details", "Book", new { id = bookResult.Id });
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public IActionResult AddToBookshelf(int bookshelfId, int bookId)
        {
            var user = this.usersService.GetByUsername(this.User.Identity.Name);
            var bookResult = this.bookService.GeBookById(bookId);
            //TODO: create addtobookshelf viewmodel and validate there
            ValidateBookExists(bookResult);
            ValidateUserBookshelfIdExists(bookshelfId, user);
            ValidateBookIsntInBookshelf(bookId, bookshelfId);

            if (ModelState.IsValid)
            {
                this.OnPostAddToBookshelf(bookshelfId, bookResult, user);
                return RedirectToAction("Details", "Bookshelf", new { id = bookshelfId });
            }

            return this.RedirectToAction("/", "Home");
        }

        public void OnPostAddToBookshelf(int bookshelfId, Book book, HoneypotUser user)
        {
            var bookBookshelf = new BookBookshelf()
            {
                BookId = book.Id,
                BookshelfId = bookshelfId
            };

            user.CustomBookshelves.First(x => x.Id == bookshelfId).Books.Add(bookBookshelf);
            this.context.SaveChanges();
        }

        public Book OnPostCreateBook(CreateBookViewModel viewModel)
        {
            var bookAuthor = this.context.Authors.FirstOrDefault(x => x.FirstName == viewModel.AuthorFirstName && x.LastName == viewModel.AuthorLastName);
            var book = this.mapper.Map<Book>(viewModel);
            book.AuthorId = bookAuthor.Id;
            this.context.Books.Add(book);
            this.context.SaveChanges();
            return book;
        }

        //INPUT DATA VALIDATION METHODS
        public void ValidateBookTitleDoesntExist(CreateBookViewModel viewModel)
        {
            if (this.bookService.BookTitleExists(viewModel.Title, viewModel.AuthorFirstName, viewModel.AuthorLastName))
            {
                ModelState.AddModelError("Title", "Book already exists.");
            }
        }

        public void ValidateAuthorNamesExist(CreateBookViewModel viewModel)
        {
            if (!this.authorService.AuthorExists(viewModel.AuthorFirstName, viewModel.AuthorLastName))
            {
                ModelState.AddModelError("AuthorFirstName", "Author doesn't exist.");
                ModelState.AddModelError("AuthorLastName", "Author doesn't exist.");
            }
        }

        public void ValidateUserBookshelfIdExists(int bookshelfId, HoneypotUser user)
        {
            if (this.bookshelfService.FindUserBookshelfById(bookshelfId, user.Id) == null)
            {
                var errorMessage = string.Format(ControllerConstants.DoesntExist, typeof(Bookshelf).Name);
                ModelState.AddModelError("Bookshelf", errorMessage);
            }
        }

        public void ValidateBookExists(Book book)
        {
            if (book == null)
            {
                var errorMessage = string.Format(ControllerConstants.DoesntExist, typeof(Book).Name);
                ModelState.AddModelError("Book", errorMessage);
            }
        }

        public void ValidateBookIsntInBookshelf(int bookId, int bookshelfId)
        {
            if (this.bookshelfService.IsBookInBookshelf(bookId, bookshelfId))
            {
                ModelState.AddModelError("Book", "Book is already on that bookshelf!");
            }
        }
    }
}