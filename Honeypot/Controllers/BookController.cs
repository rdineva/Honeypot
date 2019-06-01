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
        private readonly IRatingService ratingService;

        public BookController(HoneypotDbContext context, IMapper mapper, IUserService usersService, IAuthorService authorService, IBookshelfService bookshelfService, IBookService bookService, IRatingService ratingService)
            : base(context, mapper)
        {
            this.usersService = usersService;
            this.authorService = authorService;
            this.bookshelfService = bookshelfService;
            this.bookService = bookService;
            this.ratingService = ratingService;
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

        public void ValidateBookIsntInBookshelf(int bookId, int bookshelfId)
        {
            if (this.bookshelfService.IsBookInBookshelf(bookId, bookshelfId))
            {
                ModelState.AddModelError("Book", "Book is already on that bookshelf!");
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

        public void ValidateStars(int stars)
        {
            if (stars < 1 || stars > 5)
            {
                var errorMessage = string.Format(ControllerConstants.InvalidRating, typeof(Book).Name);
                ModelState.AddModelError("Book", errorMessage);
            }
        }

        [HttpPost]
        public IActionResult Rate(int stars, int bookId)
        {
            ValidateStars(stars);
            var book = this.bookService.GeBookById(bookId);
            ValidateBookExists(book);

            if (ModelState.IsValid)
            {
                OnPostUserRateBook(bookId, stars);
            }

            return RedirectToAction("Details", new { id = book.Id });
        }

        public void OnPostUserRateBook(int bookId, int stars)
        {
            var user = this.usersService.GetByUsername(this.User.Identity.Name);
            var userHasRatedBook = this.ratingService.HasUserRatedBook(user.Id, bookId);
            if (userHasRatedBook)
            {
                ChangeRating(user, bookId, stars);
            }
            else
            {
                AddNewRating(user, bookId, stars);
            }

            this.context.SaveChanges();
        }

        public void ChangeRating(HoneypotUser user, int bookId, int stars)
        {
            var rating = this.ratingService.FindUserBookRating(user.Id, bookId);
            rating.Stars = stars;
        }

        public void AddNewRating(HoneypotUser user, int bookId, int stars)
        {
            var rating = new Rating()
            {
                Stars = stars, UserId = user.Id, BookId = bookId 
            };

            this.context.Ratings.Add(rating);
        }
    }
}