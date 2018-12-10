using AutoMapper;
using Honeypot.Data;
using Honeypot.Models;
using Honeypot.ViewModels.Quote;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Honeypot.Controllers
{
    public class QuoteController : Controller
    {
        private readonly IMapper mapper;
        private readonly HoneypotDbContext context;

        public QuoteController(IMapper mapper, HoneypotDbContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateViewModel viewModel)
        {
            var quote = mapper.Map<Quote>(viewModel);

            this.context.Quotes.Add(quote);
            this.context.SaveChanges();

            return this.RedirectToAction("Details", "Quote", quote.Id);
        }

        public IActionResult Details(int id)
        {
            var quoteResult = this.context.Quotes.FirstOrDefaultAsync(q => q.Id == id).Result;

            if (quoteResult == null)
                return this.NotFound("No such quote found!");

            var author = this.context.Authors.FirstOrDefaultAsync(x => x.Id == quoteResult.AuthorId).Result;
            var book = this.context.Books.FirstOrDefaultAsync(x => x.Id == quoteResult.BookId).Result;

            var quote = new DetailsViewModel()
            {
                AuthorName = author.FirstName + " " + author.LastName,
                BookTitle = book.Title,
                Text =  quoteResult.Text
            };

            return this.View(quote);
        }
    }
}