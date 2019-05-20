using Honeypot.Models.MappingModels;
using System.Collections.Generic;

namespace Honeypot.ViewModels.Bookshelf
{
    public class DetailsViewModel
    {
        public string Title { get; set; }

        public virtual ICollection<BookBookshelf> Books { get; set; }

        public string OwnerId { get; set; }

        public string OwnerNickname { get; set; }

        public int BooksCount => this.Books.Count;
    }
}