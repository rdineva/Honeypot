using System.ComponentModel.DataAnnotations;

namespace Honeypot.ViewModels.Quote
{
    public class CreateViewModel
    {
        [Required]
        [StringLength(1000, ErrorMessage = "Quote length the exceeded maximum!")]
        [DataType(DataType.Text)]
        public string Text { get; set; }

        [Required]
        public Models.Author Author { get; set; }

        [Required]
        public Models.Book Book { get; set; }
    }
}
