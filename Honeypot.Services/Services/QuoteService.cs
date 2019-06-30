using System.Collections.Generic;
using Honeypot.Data;
using System.Linq;
using Honeypot.Models;
using Honeypot.Models.MappingModels;
using Honeypot.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Honeypot.Services
{
    public class QuoteService : BaseService, IQuoteService
    {
        public QuoteService(HoneypotDbContext context)
            : base(context)
        {
        }

        public bool HasUserLikedQuote(int quoteId, string userId)
        {
            bool hasUserLikedQuote = this.context
                .UsersQuotes
                .Any(x => x.UserId == userId
                       && x.QuoteId == quoteId);

            return hasUserLikedQuote;
        }

        public List<Quote> GetLikedQuotesByUser(HoneypotUser user)
        {
            var likedQuotesByUser = this.context
                .UsersQuotes
                .Include(x => x.Quote)
                .ThenInclude(x => x.Book)
                .ThenInclude(x => x.Author)
                .Where(x => x.User == user)
                .ToList()
                .ConvertAll(x => x.Quote);

            return likedQuotesByUser;
        }

        public bool QuoteExists(string quote)
        {
            var quoteExists = this.context
                .Quotes
                .Any(x => x.Text == quote);

            return quoteExists;
        }

        public Quote GetQuoteById(int id)
        {
            var quote = this.context
                .Quotes
                .Include(x => x.Author)
                .Include(x => x.Book)
                .FirstOrDefault(x => x.Id == id);

            return quote;
        }

        public UserQuote GetUsersLikedQuoteById(int quoteId, string userId)
        {
            var quote = this.context
                .UsersQuotes
                .FirstOrDefault(x => x.QuoteId == quoteId && x.UserId == userId);

            return quote;
        }
    }
}