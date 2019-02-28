using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Honeypot.ViewModels.Account
{
    public class ProfileViewModel
    {
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public ICollection<Models.Bookshelf> Bookshelves { get; set; }

        public ICollection<Models.Quote> FavouriteQuotes { get; set; }
    }
}
