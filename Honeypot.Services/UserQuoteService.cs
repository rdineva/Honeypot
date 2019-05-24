﻿using Honeypot.Data;
using System.Linq;

namespace Honeypot.Services
{
    public class UserQuoteService : BaseService
    {
        public UserQuoteService(HoneypotDbContext context) 
            : base(context)
        {
        }

        public bool HasUserLikedQuote(int quoteId, string userId)
        {
            bool hasUserLikedQuote = this.Context.UsersQuotes.Any(x => x.UserId == userId && x.QuoteId == quoteId);
            return hasUserLikedQuote;
        }
    }
}