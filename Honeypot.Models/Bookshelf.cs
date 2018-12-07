using System.Collections.Generic;
using Honeypot.Models.MappingModels;

namespace Honeypot.Models
{
    public class Bookshelf
    {
        public Bookshelf()
        {
            this.Books = new List<BooksBookshelves>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public ICollection<BooksBookshelves> Books { get; set; }

        public string OwnerId { get; set; }
        public HoneypotUser Owner { get; set; }
    }
}
