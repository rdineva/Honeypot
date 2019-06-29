using System.Collections.Generic;

namespace Honeypot.ViewModels.Quote
{
    public class MyLikedQuotesViewModel
    {
        public MyLikedQuotesViewModel()
        {
            this.Quotes = new List<Models.Quote>();
        }

        public ICollection<Models.Quote> Quotes { get; set; }
    }
}
