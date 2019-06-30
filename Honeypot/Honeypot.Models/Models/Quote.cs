using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Honeypot.Models.MappingModels;
using Honeypot.Models.Models;

namespace Honeypot.Models
{
    public class Quote : BaseModel
    {
        public Quote()
        { 
            this.LikedByUsers = new List<UserQuote>();
        }

        public string Text { get; set; }

        [ForeignKey("Author")]
        public int AuthorId { get; set; }
        public Author Author { get; set; }

        [ForeignKey("Book")]
        public int BookId { get; set; }
        public Book Book { get; set; }

        public virtual ICollection<UserQuote> LikedByUsers { get; set; }
    }
}
