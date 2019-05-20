using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Honeypot.ViewModels.Quote
{
    public class CreateViewModel
    {
        [Required]
        [StringLength(1000, ErrorMessage = "Quote should be between 5 and 1000 characters!", MinimumLength = 5)]
        [DataType(DataType.Text)]
        [DisplayName("Quote")]
        public string Text { get; set; }

        [Required]
        [DisplayName("Author")]
        public int AuthorId { get; set; }

        [Required]
        [DisplayName("Book")]
        public int BookId { get; set; }
    }
}
