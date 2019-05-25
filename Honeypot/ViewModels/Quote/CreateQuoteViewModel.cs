using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Honeypot.ViewModels.Quote
{
    public class CreateQuoteViewModel
    {
        [Required]
        [StringLength(ViewModelConstants.MaxQuoteTextLength, ErrorMessage = ViewModelConstants.StringLengthError, MinimumLength = ViewModelConstants.MinQuoteTextLength)]
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
