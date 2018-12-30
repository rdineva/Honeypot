using System.Collections.Generic;
using System.ComponentModel;

namespace Honeypot.ViewModels.Book
{
    public class BookDetailsViewModel
    {
        public string Title { get; set; }

        [DisplayName("Author")]
        public string AuthorName { get; set; }

        public int AuthorId { get; set; }

        public string Summary { get; set; }

        [DisplayName("Reviews Count")]
        public int ReviewsCount { get; set; }

        public decimal Rating { get; set; }

        public ICollection<Models.Quote> Quotes { get; set; }
    }
}
