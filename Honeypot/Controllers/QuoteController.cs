using System.Collections.Generic;
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
    [Authorize]
    public class QuoteController : BaseController
    {
        private readonly UserManager<HoneypotUser> userManager;

        public QuoteController(HoneypotDbContext context, IMapper mapper, UserManager<HoneypotUser> userManager)
            : base(context, mapper)
        {
            this.userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(CreateQuoteViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var book = this.context.Books.FirstOrDefaultAsync(x => x.Id == viewModel.BookId).Result;
                var author = this.context.Authors.FirstOrDefaultAsync(x => x.Id == viewModel.AuthorId).Result;
                if (book == null || author == null)
                {
                    return this.BadRequest("Book or author doesn't exist!");
                }

                var quoteExists = this.context.Quotes.FirstOrDefaultAsync(x => x.Text == viewModel.Text).Result != null;
                if (quoteExists)
                {
                    return this.BadRequest("Quote already exists!");
                }

                var createdQuote = this.OnPostCreateQuote(viewModel);
                return this.RedirectToAction("Details", createdQuote.Id);
            }

            return this.View(viewModel);
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
            var usersLikedQuotes = FindUsersLikedQuotes(user);
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
            user.FavouriteQuotes.Add(userQuote);
            quote.LikedByUsers.Add(userQuote);
            this.context.SaveChanges();
        }

        public List<Quote> FindUsersLikedQuotes(HoneypotUser user)
        {
            var usersLikedQuotes = this.context
                .UsersQuotes
                .Include(x => x.Quote)
                .ThenInclude(x => x.Book)
                .ThenInclude(x => x.Author)
                .Where(x => x.UserId == user.Id)
                .ToList().ConvertAll(x => x.Quote);

            return usersLikedQuotes;
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