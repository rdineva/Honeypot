using System.Collections.Generic;

namespace Honeypot.Models
{
    public class Author
    {
        public Author()
        {
            this.Books = new List<Book>();
            this.Quotes = new List<Quote>();
        }

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Biography { get; set; }

        public virtual ICollection<Book> Books { get; set; }

        public virtual ICollection<Quote> Quotes { get; set; }

        //TODO: add photo property
    }
}