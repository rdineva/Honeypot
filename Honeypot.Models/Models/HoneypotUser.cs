using System.Collections.Generic;
using Honeypot.Models.Contracts;
using Honeypot.Models.MappingModels;
using Microsoft.AspNetCore.Identity;

namespace Honeypot.Models
{
    public class HoneypotUser : IdentityUser, IPerson
    {
        public HoneypotUser()
        {
            this.CustomBookshelves = new List<Bookshelf>();
            this.LikedQuotes = new List<UserQuote>();
        }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public virtual ICollection<Bookshelf> CustomBookshelves { get; set; }

        public virtual ICollection<UserQuote> LikedQuotes { get; set; }

        //TODO: add read, to-read, and fav bookshelves
        //public int ReadId { get; set; }
        //public Bookshelf Read { get; set; }

        //public int ToReadId { get; set; }
        //public Bookshelf ToRead { get; set; }

        //public int FavouritesId { get; set; }
        //public Bookshelf Favourites { get; set; }
    }
}
