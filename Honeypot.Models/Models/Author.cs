using System.Collections.Generic;
using Honeypot.Models.Contracts;
using Honeypot.Models.Models;

namespace Honeypot.Models
{
    public class Author : BaseModel, IPerson
    {
        public Author()
        {
            this.Books = new List<Book>();
            this.Quotes = new List<Quote>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Biography { get; set; }

        public virtual ICollection<Book> Books { get; set; }

        public virtual ICollection<Quote> Quotes { get; set; }
    }
}