using System.Collections.Generic;

namespace Honeypot.Models.Models
{
    public class CustomBookshelves
    {
        public CustomBookshelves()
        {
            this.Bookshelves = new List<Bookshelf>();
        }

        public virtual ICollection<Bookshelf> Bookshelves { get; set; }
    }
}