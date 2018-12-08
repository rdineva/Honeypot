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
            this.CustomBookshelves = new List<Bookshelf>();
            this.FavouriteQuotes = new List<UsersQuotes>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<Review> Reviews { get; set; }


        //TODO: add read, to-read, and fav bookshelves
        //public int ReadId { get; set; }
        //public Bookshelf Read { get; set; }

        //public int ToReadId { get; set; }
        //public Bookshelf ToRead { get; set; }

        //public int FavouritesId { get; set; }
        //public Bookshelf Favourites { get; set; }

        public ICollection<Bookshelf> CustomBookshelves { get; set; }

        public ICollection<UsersQuotes> FavouriteQuotes { get; set; }
    }
}
