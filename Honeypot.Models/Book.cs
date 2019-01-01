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
            this.Ratings = new List<Rating>();
            this.Bookshelves = new List<BooksBookshelves>();
            this.Quotes = new List<Quote>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public virtual Author Author { get; set; }

        public string Summary { get; set; }

        public int RatingsCount => this.Ratings.Count;

        public double Rating => this.RatingsCount > 0 ? (this.Ratings.Sum(x => x.Stars) / (double)this.RatingsCount) : 0;

        public virtual ICollection<Rating> Ratings { get; set; }

        public virtual ICollection<BooksBookshelves> Bookshelves { get; set; }

        public virtual ICollection<Quote> Quotes { get; set; }
    }
}
