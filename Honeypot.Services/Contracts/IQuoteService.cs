using System.Collections.Generic;
using Honeypot.Models;

namespace Honeypot.Services.Contracts
{
    public interface IQuoteService
    {
        bool HasUserLikedQuote(int quoteId, string userId);

        List<Quote> FindUsersLikedQuotes(HoneypotUser user);

        bool QuoteExists(string quote);

        Quote GetQuoteById(int id);
    }
}
