using AutoMapper;
using Honeypot.Data;
using Honeypot.Models;
using Honeypot.Models.MappingModels;
using Honeypot.ViewModels.Quote;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Honeypot.Constants;
using Honeypot.Services.Contracts;

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
                var createdQuote = this.OnPostCreateQuote(viewModel);
                return this.RedirectToAction("Details", createdQuote.Id);
            }

            return this.View(viewModel);
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var quoteResult = this.quoteService.GetQuoteById(id);
            if (quoteResult == null)
            {
                return this.RedirectToAction("/", "Home");
            }

            var quote = this.mapper.Map<QuoteDetailsViewModel>(quoteResult);
            return this.View(quote);
        }

        [HttpPost]
        public IActionResult LikeOrUnlike(int quoteId)
        {
            var quote = this.quoteService.GetQuoteById(quoteId);
            ValidateQuoteExists(quote);

            if (ModelState.IsValid)
            {
                this.OnPostLikeOrUnlike(quote);
                return RedirectToAction("Details", new { id = quoteId });
            }

            return this.RedirectToAction("/", "Home");
        }

        public IActionResult MyLikedQuotes()
        {
            var user = userManager.GetUserAsync(HttpContext.User).Result;
            var userLikedQuotes = this.quoteService.FindUsersLikedQuotes(user);
            var quotes = new MyLikedQuotesViewModel()
            {
                Quotes = userLikedQuotes
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

        //INPUT DATA VALIDATION METHODS
        public void ValidateQuoteExists(Quote quote)
        {
            if (quote == null)
            {
                var errorMessage = string.Format(GeneralConstants.DoesntExist, typeof(Quote).Name);
                ModelState.AddModelError("Quote", errorMessage);
            }
        }
    }
}