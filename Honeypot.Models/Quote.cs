using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Honeypot.Models.MappingModels;

namespace Honeypot.Models
{
    public class Quote
    {
        public Quote(string text, int bookId, int authorId)
        {
            this.Text = text;
            this.BookId = bookId;
            this.AuthorId = authorId;
            this.LikedByUsers = new List<UserQuote>();
        }

        public int Id { get; set; }

        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public virtual Author Author { get; set; }

        public string Text { get; private set; }

        [ForeignKey("Book")]
        public int BookId { get; set; }
        public virtual Book Book { get; set; }

        public ICollection<UserQuote> LikedByUsers { get; set; }
    }
}
