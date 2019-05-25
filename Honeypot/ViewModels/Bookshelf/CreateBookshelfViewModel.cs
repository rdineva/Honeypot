using System.ComponentModel.DataAnnotations;

namespace Honeypot.ViewModels.Bookshelf
{
    public class CreateBookshelfViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [StringLength(ViewModelConstants.MaxTitleLength, ErrorMessage =ViewModelConstants.StringLengthError, MinimumLength = ViewModelConstants.MinTitleLength)]
        public string Title { get; set; }
    }
}
