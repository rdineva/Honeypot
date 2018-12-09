using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Honeypot.Models;

namespace Honeypot.ViewModels.Account
{
    public class ProfileViewModel
    {
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }

        [DisplayName("Username")]
        public string Username { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        public ICollection<Bookshelf> Bookshelves { get; set; }

        public ICollection<Models.Quote> FavouriteQuotes { get; set; }
    }
}
