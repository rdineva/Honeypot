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
using Honeypot.Services.Contracts;
using Honeypot.ViewModels;

namespace Honeypot.Controllers
{
    [Authorize]
    public class QuoteController : BaseController
    {
        private readonly UserManager<HoneypotUser> userManager;
        private readonly IAuthorService authorService;
        private readonly IBookService bookService;
        private readonly IQuoteService quoteService;

        public QuoteController(HoneypotDbContext context, UserManager<HoneypotUser> userManager, IMapper mapper, IAuthorService authorService, IBookService bookService, IQuoteService quoteService)
            : base(context, mapper)
        {
            this.userManager = userManager;
            this.authorService = authorService;
            this.bookService = bookService;
            this.quoteService = quoteService;
        }

        [Authorize(Roles = Role.Admin)]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public IActionResult Create(CreateQuoteViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var error = CheckQuoteCreateErrors(viewModel);
                if (error == null)
                {
                    var createdQuote = this.OnPostCreateQuote(viewModel);
                    return this.RedirectToAction("Details", createdQuote.Id);
                }

                return this.View("Error", new ErrorViewModel()); //TODO: add error text
            }

            return this.View(viewModel);
        }

        public string CheckQuoteCreateErrors(CreateQuoteViewModel viewModel)
        {
            if (this.authorService.GeAuthorById(viewModel.AuthorId) == null)
            {
                return "Author doesn't exist!";
            }

            if (this.bookService.GeBookById(viewModel.BookId) == null)
            {
                return "Book doesn't exist!";
            }

            if (this.quoteService.QuoteExists(viewModel.Text))
            {
                return "Quote already exists!";
            }

            return string.Empty;
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var quoteResult = this.context.Quotes.Include(x => x.Author).Include(x => x.Book).FirstOrDefaultAsync(q => q.Id == id).Result;
            if (quoteResult == null)
            {
                return this.NotFound("No such quote found!");
            }

            var quote = this.mapper.Map<QuoteDetailsViewModel>(quoteResult);
            return this.View(quote);
        }

        [HttpPost]
        public IActionResult LikeOrUnlike(int quoteId)
        {
            var quote = this.context.Quotes.FirstOrDefaultAsync(x => x.Id == quoteId).Result;
            if (quote == null)
            {
                return this.BadRequest("No such quote exists!");
            }

            this.OnPostLikeOrUnlike(quote);
            return RedirectToAction("Details", new { id = quoteId });
        }

        public IActionResult MyLikedQuotes()
        {
            var user = userManager.GetUserAsync(HttpContext.User).Result;
            var usersLikedQuotes = this.quoteService.FindUsersLikedQuotes(user);
            var quotes = new MyLikedQuotesViewModel()
            {
                Quotes = usersLikedQuotes
            };

            return this.View(quotes);
        }

        public Quote OnPostCreateQuote(CreateQuoteViewModel viewModel)
        {
            var quote = this.mapper.Map<Quote>(viewModel);
            quote.AuthorId = viewModel.AuthorId;
            quote.BookId = viewModel.BookId;
            this.context.Quotes.Add(quote);
            this.context.SaveChanges();
            return quote;
        }

        public void OnPostLikeQuote(Quote quote, HoneypotUser user)
        {
            var userQuote = new UserQuote()
            {
                QuoteId = quote.Id,
                UserId = user.Id
            };

            this.context.UsersQuotes.Add(userQuote);
            user.LikedQuotes.Add(userQuote);
            quote.LikedByUsers.Add(userQuote);
            this.context.SaveChanges();
        }

        public void OnPostUnlikeQuote(Quote quote, HoneypotUser user)
        {
            var quoteToUnlike =
                this.context.UsersQuotes.FirstOrDefault(x => x.QuoteId == quote.Id && x.UserId == user.Id);
            this.context.UsersQuotes.Remove(quoteToUnlike);
            this.context.SaveChanges();
        }

        public void OnPostLikeOrUnlike(Quote quote)
        {
            var user = userManager.GetUserAsync(HttpContext.User).Result;
            var isQuoteliked = this.context.UsersQuotes.Any(x => x.QuoteId == quote.Id && x.UserId == user.Id);

            if (isQuoteliked)
            {
                this.OnPostUnlikeQuote(quote, user);
            }
            else
            {
                this.OnPostLikeQuote(quote, user);
            }
        }
    }
}