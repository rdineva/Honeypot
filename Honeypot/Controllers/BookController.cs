using AutoMapper;
using Honeypot.Data;
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

            var book = mapper.Map<BookDetailsViewModel>(bookResult);

            var authorName = context.Authors.FirstOrDefaultAsync(x => x.Id == bookResult.AuthorId).Result;
            book.Author = authorName.FirstName + " " + authorName.LastName;

            return View(book);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateViewModel viewModel)
        {
            return null;
        }
    }
}