using System.Collections.Generic;
using Honeypot.Models.MappingModels;

namespace Honeypot.Models
{
    public class Quote
    {
        public Quote()
        {
            this.LikedByUsers = new List<UsersQuotes>();
        }

        public int Id { get; set; }

        public int AuthorId { get; set; }
        public virtual Author Author { get; set; }

        public string Text { get; set; }

        public int BookId { get; set; }
        public virtual Book Book { get; set; }

        public ICollection<UsersQuotes> LikedByUsers { get; set; }
    }
}
