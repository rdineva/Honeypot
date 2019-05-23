using Honeypot.Models.MappingModels;
using System.Collections.Generic;

namespace Honeypot.ViewModels.Bookshelf
{
    public class BookshelfDetailsViewModel
    {
        public string Title { get; set; }

        public virtual ICollection<BookBookshelf> Books { get; set; }

        public string UserId { get; set; }

        public string UserNickname { get; set; }

        public int BooksCount => this.Books.Count;
    }
}