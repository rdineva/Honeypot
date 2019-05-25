using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Honeypot.ViewModels.Book
{
    public class CreateBookViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [StringLength(ErrorConstants.MaxTitleLength, ErrorMessage =ErrorConstants.StringLengthError, MinimumLength = ErrorConstants.MinTitleLength)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(ErrorConstants.MaxSummaryLength, ErrorMessage = ErrorConstants.StringLengthError, MinimumLength = ErrorConstants.MinSummaryLength)]
        public string Summary { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [DisplayName("Author's First Name")]
        [StringLength(ErrorConstants.MaxNameLength, ErrorMessage = ErrorConstants.StringLengthError, MinimumLength = ErrorConstants.MinNameLength)]
        public string AuthorFirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [DisplayName("Author's Last Name")]
        [StringLength(ErrorConstants.MaxNameLength, ErrorMessage = ErrorConstants.StringLengthError, MinimumLength = ErrorConstants.MinNameLength)]
        public string AuthorLastName { get; set; }
    }
}