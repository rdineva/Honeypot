using AutoMapper;
using Honeypot.Data;
using Honeypot.Models;
using Honeypot.Models.MappingModels;
using Honeypot.ViewModels.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var bookResult = this.context.Books
                .Include(x => x.Author)
                .Include(x => x.Quotes)
                .FirstOrDefaultAsync(x => x.Id == id).Result;

            if (bookResult == null)
            {
                return this.NotFound("No such book found.");
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
            if (ModelState.IsValid)
            {
                if (this.authorService.AuthorExists(viewModel.AuthorFirstName, viewModel.AuthorLastName))
                {
                    return this.BadRequest("Author doesn't exist!");
                }

                if (this.bookService.BookTitleExists(viewModel.Title, viewModel.AuthorFirstName, viewModel.AuthorLastName))
                {
                    return this.BadRequest("Book already exists!");
                }

                var bookResult = OnPostCreateBook(viewModel);
                return RedirectToAction("Details", "Book", new { id = bookResult.Id });
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public IActionResult AddToBookshelf(int bookshelfId, int bookId)
        {
            var bookResult = this.bookService.GeBookById(bookId);
            if (bookResult == null)
            {
                return this.BadRequest("Book doesn't exist!");
            }

            var user = this.usersService.GetByUsername(this.User.Identity.Name);
            if (this.bookshelfService.FindUserBookshelfById(bookshelfId, user.Id) == null)
            {
                return this.BadRequest("Bookshelf doesn't exist!");
            }

            if (this.bookshelfService.IsBookInBookshelf(bookId, bookshelfId))
            {
                return this.BadRequest("Book is already on that bookshelf!");
            }

            this.OnPostAddToBookshelf(bookshelfId, bookResult, user);
            return RedirectToAction("Details", "Bookshelf", new { id = bookshelfId });
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

        //[HttpPost]
        //public IActionResult Rate(int stars, int bookId)
        //{
        //    var book = this.context.Books.FirstOrDefaultAsync(x => x.Id == bookId).Result;
        //    if (book == null)
        //    {
        //        return this.BadRequest("Book is invalid!");
        //    }

        //    if (stars < 1 || stars > 5)
        //    {
        //        return this.BadRequest("Rating is invalid!");
        //    }

        //    var user = this.usersService.GetByUsername(this.User.Identity.Name);
        //    var hasUserRatedBook = this.context.Ratings.Any(x => x.BookId == bookId && x.UserId == user.Id);

        //    if (hasUserRatedBook)
        //    {
        //        var rating = this.context.Ratings.FirstOrDefaultAsync(x => x.BookId == bookId && x.UserId == user.Id).Result;
        //       // rating.Stars = stars;
        //    }
        //    else
        //    {
        //        var rating = new Rating() { Stars = stars, UserId = user.Id };
        //        this.context.Books.FirstOrDefaultAsync(x => x.Id == bookId).Result.Ratings.Add(rating);
        //    }

        //    this.context.SaveChanges();

        //    return RedirectToAction("Details", new { id = book.Id });
        //}

        public Book OnPostCreateBook(CreateBookViewModel viewModel)
        {
            var bookAuthor = this.context.Authors.FirstOrDefault(x =>
                x.FirstName == viewModel.AuthorFirstName && x.LastName == viewModel.AuthorLastName);

            var book = this.mapper.Map<Book>(viewModel);
            book.AuthorId = bookAuthor.Id;
            this.context.Books.Add(book);
            this.context.SaveChanges();
            return book;
        }
    }
}