using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Honeypot.Models.MappingModels;

namespace Honeypot.Models
{
    public class Quote
    {
        public Quote()
        {
            this.UsersWhoLikedIt = new List<UsersQuotes>();
        }

        public int Id { get; set; }

        [ForeignKey("Author")]
        public int? AuthorId { get; set; }
        public virtual Author Author { get; set; }

        public string Text { get; set; }

        [ForeignKey("Book")]
        public int? BookId { get; set; }
        public virtual Book Book { get; set; }

        public virtual ICollection<UsersQuotes> UsersWhoLikedIt { get; set; }
    }
}
