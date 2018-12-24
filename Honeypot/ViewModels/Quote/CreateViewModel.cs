using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Honeypot.ViewModels.Quote
{
    public class CreateViewModel
    {
        [Required]
        [StringLength(1000, ErrorMessage = "Quote length exceeded the maximum!")]
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
