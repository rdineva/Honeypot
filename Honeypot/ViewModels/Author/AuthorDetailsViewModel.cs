using System.Collections.Generic;
using Honeypot.Models;

namespace Honeypot.ViewModels.Author
{
    public class AuthorDetailsViewModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Biography { get; set; }

        public ICollection<Models.Book> Books { get; set; }

        public ICollection<Quote> Quotes { get; set; }
    }
}
