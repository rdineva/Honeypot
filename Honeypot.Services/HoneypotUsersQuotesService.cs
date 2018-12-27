using Honeypot.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Honeypot.Services
{
    public class HoneypotUsersQuotesService
    {
        private readonly HoneypotDbContext context;

        public HoneypotUsersQuotesService(HoneypotDbContext context)
        {
            this.context = context;
        }

        public bool HasUserLikedQuote(int quoteId, string userId)
        {
            if (this.context.UsersQuotes.Any(x => x.UserId == userId && x.QuoteId == quoteId))
                return true;

            return false;
        }
    }
}
