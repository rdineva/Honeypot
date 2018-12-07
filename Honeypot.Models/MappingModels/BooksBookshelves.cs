namespace Honeypot.Models.MappingModels
{
    public class BooksBookshelves
    {
        public int BookId { get; set; }
        public Book Book { get; set; }

        public int BookshelfId { get; set; }
        public Bookshelf Bookshelf { get; set; }
    }
}
