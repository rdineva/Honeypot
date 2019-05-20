using AutoMapper;
using Honeypot.Data;
using Honeypot.Models;
using Honeypot.Models.MappingModels;
using Honeypot.ViewModels.Quote;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Honeypot.Controllers
{
    //TODO: simplify methods aka use abstraction
    [Authorize]
    public class QuoteController : Controller
    {
        private readonly IMapper mapper;
        private readonly HoneypotDbContext context;
        private readonly UserManager<HoneypotUser> userManager;

        public QuoteController(IMapper mapper, HoneypotDbContext context, UserManager<HoneypotUser> userManager)
        {
            this.mapper = mapper;
            this.context = context;
            this.userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(CreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return this.View(viewModel);
            }

            var quoteText = viewModel.Text;
            var quoteBookId = viewModel.BookId;
            var quoteAuthorId = viewModel.AuthorId;

            var quote = new Quote(quoteText, quoteBookId, quoteAuthorId);

            var book = this.context.Books.FirstOrDefaultAsync(x => x.Id == quote.BookId).Result;
            var author = this.context.Authors.FirstOrDefaultAsync(x => x.Id == quote.AuthorId).Result;

            if (book == null || author == null)
                return this.BadRequest("Book or author doesn't exist!");

            if (this.context.Quotes.FirstOrDefaultAsync(x => x.Text == quote.Text).Result != null)
                return this.BadRequest("Quote already exists!");

            book.Quotes.Add(quote);
            author.Quotes.Add(quote);
            this.context.Quotes.Add(quote);
            this.context.SaveChanges();

            return this.RedirectToAction("Details", quote.Id);
        }

        [AllowAnonymous]
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
                Text = quoteResult.Text,
                BookId = book.Id,
                AuthorId = author.Id,
                Id = id
            };

            return this.View(quote);
        }

        [HttpPost]
        public IActionResult Favourite(int id)
        {
            var user = userManager.GetUserAsync(HttpContext.User).Result;

            var quote = this.context.Quotes.FirstOrDefaultAsync(x => x.Id == id).Result;

            if (quote == null)
                return this.BadRequest("No such quote exists!");

            if (user.FavouriteQuotes.Any(x => x.QuoteId == id))
                return RedirectToAction("Details", new { id = id });

            var userQuote = new UserQuote() { QuoteId = id, UserId = user.Id };
            this.context.UsersQuotes.Add(userQuote);
            user.FavouriteQuotes.Add(userQuote);
            quote.LikedByUsers.Add(userQuote);

            this.context.SaveChanges();

            return RedirectToAction("Details", new { id = id });
        }

        public IActionResult MyFavouriteQuotes()
        {
            var user = userManager.GetUserAsync(HttpContext.User).Result;
            var quotesList = this.context.UsersQuotes.Where(x => x.UserId == user.Id).Select(x => x.QuoteId).ToList();

            var quotes = new MyFavouriteQuotesViewModel();
            foreach(var q in quotesList)
            {
                var quote = this.context.Quotes.FirstOrDefaultAsync(x => x.Id == q).Result;
                var author = this.context.Authors.FirstOrDefaultAsync(x => x.Id == quote.AuthorId).Result;
                quote.Author = author;
                quotes.Quotes.Add(quote);
            }            

            return this.View(quotes);
        }

        [HttpPost]
        public IActionResult Unfavourite(int id)
        {
            var user = userManager.GetUserAsync(HttpContext.User).Result;

            var quote = this.context.Quotes.FirstOrDefaultAsync(x => x.Id == id).Result;

            if (quote == null)
            {
                return this.BadRequest("No such quote exists!");
            }

            if (user.FavouriteQuotes.All(x => x.QuoteId != id))
            {
                return RedirectToAction("Details", new {id = id});
            }

            var userQuote = new UserQuote()
            {
                QuoteId = id, UserId = user.Id
            };

            this.context.UsersQuotes.Remove(userQuote);
            user.FavouriteQuotes.Remove(userQuote);
            quote.LikedByUsers.Remove(userQuote);

            this.context.SaveChanges();

            return RedirectToAction("Details", new { id = id });
        }
    }
}