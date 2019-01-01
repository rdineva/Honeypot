using AutoMapper;
using Honeypot.Data;
using Honeypot.Models;
using Honeypot.ViewModels.Book;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Honeypot.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private readonly IMapper mapper;
        private readonly HoneypotDbContext context;

        public BookController(IMapper mapper, HoneypotDbContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public IActionResult Details(int id)
        {
            var bookResult = this.context.Books.FirstOrDefaultAsync(x => x.Id == id).Result;

            if (bookResult == null)
            {
                return this.NotFound("No such book found.");
            }

            var author = this.context.Authors.FirstOrDefaultAsync(a => a.Id == bookResult.AuthorId).Result;

            var book = new BookDetailsViewModel()
            {
                Id = bookResult.Id,
                Title = bookResult.Title,
                Summary = bookResult.Summary,
                AuthorName = author.FirstName + " " + author.LastName,
                AuthorId = bookResult.AuthorId,
                Rating = bookResult.Rating,
                ReviewsCount = bookResult.RatingsCount,
                Quotes = this.context.Quotes.Where(x => x.AuthorId == bookResult.AuthorId && x.BookId == bookResult.Id).ToList()
            };

            return View(book);
        }

        [AllowAnonymous]
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

            var author = this.context.Authors.FirstOrDefaultAsync(x =>
                x.FirstName == viewModel.AuthorFirstName && x.LastName == viewModel.AuthorLastName).Result;

            if (author == null)
                return this.BadRequest("Author doesn't exist!");

            var book = new Book()
            {
                Title = viewModel.Title.Trim(),
                Summary = viewModel.Summary.Trim(),
                AuthorId = author.Id,
                Author =  author
            };
            
            var bookExists = this.context.Books
                .FirstOrDefaultAsync(x => x.Title == book.Title && x.AuthorId == book.AuthorId).Result;

            if (bookExists != null)
                return this.BadRequest("Book already exists!");

            author.Books.Add(book);
            this.context.Books.AddAsync(book);
            this.context.SaveChanges();

            return RedirectToAction("Details","Book", new { id = book.Id });
        }

        [HttpPost]
        public IActionResult AddToBookshelf()
        {
            return null;
        }

        [HttpPost]
        public IActionResult Rate(int stars, int BookId)
        {
            var book = this.context.Books.FirstOrDefaultAsync(x => x.Id == BookId).Result;
            if (book == null)
                return this.BadRequest("Book is invalid!");

            if (stars < 1 || stars > 5)
                return this.BadRequest("Rating is invalid!");

            var rating = (Rating)Enum.Parse(typeof(Rating), stars.ToString());

            this.context.Books.FirstOrDefaultAsync(x => x.Id == BookId).Result.Ratings.Add(rating);
            this.context.SaveChanges();

            return RedirectToAction("Details", new { id = book.Id });
        }
    }
}