using System.ComponentModel.DataAnnotations;
using Honeypot.Attributes;

namespace Honeypot.ViewModels.Bookshelf
{
    public class CreateBookshelfViewModel
    {
        [Required]
        [UniqueUserBookshelf]
        [DataType(DataType.Text)]
        [StringLength(ViewModelConstants.MaxTitleLength, ErrorMessage =ViewModelConstants.StringLengthError, MinimumLength = ViewModelConstants.MinTitleLength)]
        public string Title { get; set; }
    }
}
