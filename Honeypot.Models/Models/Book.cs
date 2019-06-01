using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Honeypot.Models.MappingModels;
using Honeypot.Models.Models;

namespace Honeypot.Models
{
    public class Book : BaseModel
    {
        public Book()
        {
            this.Ratings = new List<Rating>();
            this.InBookshelves = new List<BookBookshelf>();
            this.Quotes = new List<Quote>();
        }

        public string Title { get; private set; }

        public string Summary { get; private set; }

        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; }

        public virtual ICollection<BookBookshelf> InBookshelves { get; set; }

        public virtual ICollection<Quote> Quotes { get; set; }

        public int RatingsCount()
        {
            return this.Ratings.Count;
        }

        public double AverageRating()
        {
            if (this.RatingsCount() == 0)
            {
                return 0;
            }

            var averageRating = this.Ratings.Sum(x => (int)x.Stars) / (double)this.RatingsCount();
            return averageRating;
        }
    }
}