using Honeypot.Models.MappingModels;
using System.Collections.Generic;

namespace Honeypot.ViewModels.Quote
{
    public class MyFavouriteQuotesViewModel
    {
        public MyFavouriteQuotesViewModel()
        {
            this.Quotes = new List<Models.Quote>();
        }

        public ICollection<Models.Quote> Quotes { get; set; }
    }
}
