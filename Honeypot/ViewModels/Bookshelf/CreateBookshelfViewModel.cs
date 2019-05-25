using System.ComponentModel.DataAnnotations;

namespace Honeypot.ViewModels.Bookshelf
{
    public class CreateBookshelfViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [StringLength(ErrorConstants.MaxTitleLength, ErrorMessage =ErrorConstants.StringLengthError, MinimumLength = ErrorConstants.MinTitleLength)]
        public string Title { get; set; }
    }
}
