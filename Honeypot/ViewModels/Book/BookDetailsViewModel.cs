using System.Collections.Generic;
using System.ComponentModel;
using Honeypot.Models.Enums;

namespace Honeypot.ViewModels.Book
{
    public class BookDetailsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        [DisplayName("Author")]
        public string AuthorName { get; set; }

        public int AuthorId { get; set; }

        public string Summary { get; set; }

        [DisplayName("Reviews Count")]
        public int ReviewsCount { get; set; }

        public double Rating { get; set; }

        public Genre Genre { get; set; }

        public ICollection<Models.Quote> Quotes { get; set; }
    }
}
