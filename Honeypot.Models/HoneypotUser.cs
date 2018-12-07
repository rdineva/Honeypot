using System.Collections.Generic;
using Honeypot.Models.MappingModels;
using Microsoft.AspNetCore.Identity;

namespace Honeypot.Models
{
    public class HoneypotUser : IdentityUser
    {
        public HoneypotUser()
        {
            this.Reviews = new List<Review>();
            this.Bookshelves = new List<Bookshelf>();
            this.FavouriteQuotes = new List<UsersQuotes>();
        }

        public ICollection<Review> Reviews { get; set; }

        public ICollection<Bookshelf> Bookshelves { get; set; }

        public ICollection<UsersQuotes> FavouriteQuotes { get; set; }
    }
}
