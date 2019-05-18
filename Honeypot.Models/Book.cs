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
            this.InBookshelves = new List<BooksBookshelves>();
            this.Quotes = new List<Quote>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public virtual Author Author { get; set; }

        public string Summary { get; set; }

        public ICollection<Rating> Ratings { get; set; }
               
        public ICollection<BooksBookshelves> InBookshelves { get; set; }
               
        public ICollection<Quote> Quotes { get; set; }

        public int CountOfRatings => this.Ratings.Count;

        public double AverageRating => this.CountOfRatings > 0 ? (this.Ratings.Sum(x => x.Stars) / (double)this.CountOfRatings) : 0;
    }
}