using System.Collections.Generic;

namespace Honeypot.Models
{
    public class Author 
    {
        public Author(/*string firstName, string lastName, string biography*/)
        {
            //this.FirstName = firstName;
            //this.LastName = lastName;
            //this.Biography = biography;
            this.Books = new List<Book>();
            this.Quotes = new List<Quote>();
        }

        public int Id { get; set; }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string Biography { get; private set; }

        public virtual ICollection<Book> Books { get; set; }

        public virtual ICollection<Quote> Quotes { get; set; }
    }
}