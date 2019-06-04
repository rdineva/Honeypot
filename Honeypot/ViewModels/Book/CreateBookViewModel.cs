using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Honeypot.Attributes;
using Honeypot.Models.Enums;

namespace Honeypot.ViewModels.Book
{
    public class CreateBookViewModel
    {
        [Required]
        [UniqueBookTitleByAuthor]
        [DataType(DataType.Text)]
        [StringLength(ViewModelConstants.MaxTitleLength, ErrorMessage = ViewModelConstants.StringLengthError, MinimumLength = ViewModelConstants.MinTitleLength)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(ViewModelConstants.MaxSummaryLength, ErrorMessage = ViewModelConstants.StringLengthError, MinimumLength = ViewModelConstants.MinSummaryLength)]
        public string Summary { get; set; }

        [Required]
        [AuthorNamesExists(ShouldAuthorExist = true)]
        [DataType(DataType.Text)]
        [DisplayName("Author's First Name")]
        [StringLength(ViewModelConstants.MaxNameLength, ErrorMessage = ViewModelConstants.StringLengthError, MinimumLength = ViewModelConstants.MinNameLength)]
        public string AuthorFirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [DisplayName("Author's Last Name")]
        [StringLength(ViewModelConstants.MaxNameLength, ErrorMessage = ViewModelConstants.StringLengthError, MinimumLength = ViewModelConstants.MinNameLength)]
        public string AuthorLastName { get; set; }

        [Required]
        [DisplayName("Genre")]
        public Genre Genre { get; set; }
    }
}