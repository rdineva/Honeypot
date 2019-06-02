using System.Collections.Generic;

namespace Honeypot.ViewModels.Bookshelf
{
    public class MyBookshelvesViewModel
    {
        public MyBookshelvesViewModel()
        {
            this.Bookshelves = new List<Models.Bookshelf>();
        }

        public ICollection<Models.Bookshelf> Bookshelves { get; set; }
    }
}
