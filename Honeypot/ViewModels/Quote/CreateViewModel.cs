using System.ComponentModel.DataAnnotations;

namespace Honeypot.ViewModels.Quote
{
    public class CreateViewModel
    {
        [Required]
        [StringLength(1000, ErrorMessage = "Quote length exceeded the maximum!")]
        [DataType(DataType.Text)]
        public string Text { get; set; }

        [Required]
        public int AuthorId { get; set; }

        [Required]
        public int BookId { get; set; }
    }
}
