using System.Collections.Generic;
using System.Linq;
using Honeypot.Models.MappingModels;

namespace Honeypot.Models
{
    public class Book
    {
        public Book()
        {
            this.Reviews = new List<Review>();
            this.Bookshelves = new List<BooksBookshelves>();
            this.Quotes = new List<Quote>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public string Summary { get; set; }

        public int ReviewsCount => this.Reviews.Count;

        public decimal Rating => this.ReviewsCount > 0 ? (this.Reviews.Sum(x => int.Parse(x.Rating.ToString())) 
                                 / (decimal)this.ReviewsCount) : 0;

        public ICollection<Review> Reviews { get; set; }

        public ICollection<BooksBookshelves> Bookshelves { get; set; }

        public ICollection<Quote> Quotes { get; set; }
    }
}
