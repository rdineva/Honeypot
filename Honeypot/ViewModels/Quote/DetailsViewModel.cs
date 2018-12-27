using System.ComponentModel;

namespace Honeypot.ViewModels.Quote
{
    public class DetailsViewModel
    {
        [DisplayName("Author")]
        public string AuthorName { get; set; }

        public int AuthorId { get; set; }

        public int Id { get; set; }

        public string Text { get; set; }

        [DisplayName("Book")]
        public string BookTitle { get; set; }

        public int BookId { get; set; }
    }
}
