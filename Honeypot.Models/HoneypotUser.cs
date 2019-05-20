using System.Collections.Generic;
using Honeypot.Models.Contracts;
using Honeypot.Models.MappingModels;
using Microsoft.AspNetCore.Identity;

namespace Honeypot.Models
{
    public class HoneypotUser : IdentityUser
    {
        public HoneypotUser(string firstName, string lastName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.CustomBookshelves = new List<Bookshelf>();
            this.FavouriteQuotes = new List<UserQuote>();
        }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public ICollection<Bookshelf> CustomBookshelves { get; set; }

        public ICollection<UserQuote> FavouriteQuotes { get; set; }

        //TODO: add read, to-read, and fav bookshelves
        //public int ReadId { get; set; }
        //public Bookshelf Read { get; set; }

        //public int ToReadId { get; set; }
        //public Bookshelf ToRead { get; set; }

        //public int FavouritesId { get; set; }
        //public Bookshelf Favourites { get; set; }
    }
}
