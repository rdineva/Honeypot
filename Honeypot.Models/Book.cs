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
            this.AddedToBookshelves = new List<BooksBookshelves>();
            this.Quotes = new List<Quote>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public virtual Author Author { get; set; }

        public string Summary { get; set; }

        public int CountOfRatings => this.Ratings.Count;

        public double AverageRating => this.CountOfRatings > 0 ? (this.Ratings.Sum(x => x.Stars) / (double)this.CountOfRatings) : 0;

        public virtual ICollection<Rating> Ratings { get; set; }

        public virtual ICollection<BooksBookshelves> AddedToBookshelves { get; set; }

        public virtual ICollection<Quote> Quotes { get; set; }
    }
}