using System.Collections.Generic;
using AutoMapper;
using Honeypot.Data;
using Honeypot.Models;
using Honeypot.ViewModels.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Honeypot.Models.Enums;
using Honeypot.Services.Contracts;

namespace Honeypot.Controllers
{
    [Authorize]
    public class BookController : BaseController
    {
        private readonly IBookService bookService;
        private readonly IAuthorService authorService;

        public BookController(HoneypotDbContext context, IMapper mapper, IBookService bookService, IAuthorService authorService)
            : base(context, mapper)
        {
            this.bookService = bookService;
            this.authorService = authorService;
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var bookResult = this.bookService.GetBookById(id);
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
            if (ModelState.IsValid)
            {
                var bookResult = OnPostCreateBook(viewModel);
                return RedirectToAction("Details", "Book", new { id = bookResult.Id });
            }

            return this.View(viewModel);
        }

        public IActionResult Discover()
        {
            var genres = this.bookService.GetAllGenres();
            Dictionary<Genre, List<Book>> allBooksByGenres = this.bookService.GetAllBooksFromAllGenres(genres);
            var viewModel = new  DiscoverViewModel()
            {
                BooksByGenre = allBooksByGenres
            };

            return this.View(viewModel);
        }

        private Book OnPostCreateBook(CreateBookViewModel viewModel)
        {
            var bookAuthor = this.authorService.GetAuthorByName(viewModel.AuthorFirstName, viewModel.AuthorLastName);
            var book = this.mapper.Map<Book>(viewModel);
            book.AuthorId = bookAuthor.Id;
            this.context.Books.Add(book);
            this.context.SaveChanges();
            return book;
        }
    }
}