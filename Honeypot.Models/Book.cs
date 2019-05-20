using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Honeypot.Models.MappingModels;

namespace Honeypot.Models
{
    public class Book
    {
        public Book(string title, string summary, int authorId, Author author)
        {
            this.Title = title;
            this.Summary = summary;
            this.AuthorId = authorId;
            this.Author = author;
            this.Ratings = new List<Rating>();
            this.InBookshelves = new List<BookBookshelf>();
            this.Quotes = new List<Quote>();
        }

        public int Id { get; set; }

        public string Title { get; private set; }

        public string Summary { get; private set; }

        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public Author Author { get; set; }

        public ICollection<Rating> Ratings { get; set; }
               
        public ICollection<BookBookshelf> InBookshelves { get; set; }
               
        public ICollection<Quote> Quotes { get; set; }

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