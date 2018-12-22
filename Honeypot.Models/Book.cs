using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public virtual Author Author { get; set; }

        public string Summary { get; set; }

        public int ReviewsCount => this.Reviews.Count;

        public decimal Rating => this.ReviewsCount > 0 ? (this.Reviews.Sum(x => int.Parse(x.Rating.ToString())) 
                                 / (decimal)this.ReviewsCount) : 0;

        public virtual ICollection<Review> Reviews { get; set; }

        public virtual ICollection<BooksBookshelves> Bookshelves { get; set; }

        public virtual ICollection<Quote> Quotes { get; set; }
    }
}
