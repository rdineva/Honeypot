using System.Collections.Generic;
using Honeypot.Models;
using Honeypot.Models.MappingModels;

namespace Honeypot.Services.Contracts
{
    public interface IQuoteService
    {
        bool HasUserLikedQuote(int quoteId, string userId);

        List<Quote> GetLikedQuotesByUser(HoneypotUser user);

        bool QuoteExists(string quote);

        Quote GetQuoteById(int id);

        UserQuote GetUsersLikedQuoteById(int quoteId, string userId);
    }
}
