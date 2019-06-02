using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Honeypot.Attributes;

namespace Honeypot.ViewModels.Quote
{
    public class CreateQuoteViewModel
    {
        [Required]
        [UniqueQuote]
        [DisplayName("Quote")]
        [DataType(DataType.Text)]
        [StringLength(ViewModelConstants.MaxQuoteTextLength, ErrorMessage = ViewModelConstants.StringLengthError, MinimumLength = ViewModelConstants.MinQuoteTextLength)]
        public string Text { get; set; }

        [Required]
        [AuthorExists]
        [DisplayName("Author")]
        public int AuthorId { get; set; }

        [Required]
        [BookExists]
        [DisplayName("Book")]
        public int BookId { get; set; }
    }
}
