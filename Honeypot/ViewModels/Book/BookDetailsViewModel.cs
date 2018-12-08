using System.Collections.Generic;
using Honeypot.Models;

namespace Honeypot.ViewModels.Book
{
    public class BookDetailsViewModel
    {
        public string Title { get; set; }

        public Author Author { get; set; }

        public string Summary { get; set; }

        public int ReviewsCount { get; set; }

        public decimal Rating { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
}
