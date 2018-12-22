using AutoMapper;
using Honeypot.Data;
using Honeypot.Models;
using Honeypot.ViewModels.Book;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Honeypot.Controllers
{
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
                Title = bookResult.Title,
                Summary = bookResult.Summary,
                AuthorName = author.FirstName + " " + author.LastName,
                AuthorId = bookResult.AuthorId,
                Rating = bookResult.Rating,
                Reviews = bookResult.Reviews,
                ReviewsCount = bookResult.ReviewsCount
            };

            return View(book);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateViewModel viewModel)
        {
            var author = this.context.Authors.FirstOrDefaultAsync(x =>
                x.FirstName == viewModel.AuthorFirstName && x.LastName == viewModel.AuthorLastName).Result;

            if (author == null)
                return this.BadRequest("Author doesn't exist!");

            var book = new Book()
            {
                Title = viewModel.Title,
                Summary = viewModel.Summary,
                AuthorId = author.Id,
                Author =  author
            };

            var bookExists = this.context.Books
                .FirstOrDefaultAsync(x => x.Title == book.Title && x.AuthorId == book.AuthorId).Result;

            if (bookExists != null)
                return this.BadRequest("Book already exists!");

            this.context.Books.AddAsync(book);
            this.context.SaveChanges();

            var bookId = this.context.Books.FirstOrDefaultAsync(x => x.Title == book.Title);

            return RedirectToAction("Details", bookId);
        }
    }
}