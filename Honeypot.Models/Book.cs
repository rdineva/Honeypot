using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Honeypot.Models.MappingModels;

namespace Honeypot.Models
{
    public class Book
    {
        public Book(/*string title, string summary, int authorId*/)
        {
            //this.Title = title;
            //this.Summary = summary;
            //this.AuthorId = authorId;
            this.Ratings = new List<Rating>();
            this.InBookshelves = new List<BookBookshelf>();
            this.Quotes = new List<Quote>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

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

            var averageRating = this.Ratings.Sum(x => x.Stars) / (double)this.RatingsCount();
            return averageRating;
        }
    }
}