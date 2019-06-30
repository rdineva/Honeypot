using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Honeypot.Attributes;

namespace Honeypot.ViewModels.Author
{
    public class CreateAuthorViewModel
    {
        [Required]
        [DisplayName("First Name")]
        [AuthorNamesExistAttribute(ShouldAuthorExist = false)]
        [DataType(DataType.Text)]
        [StringLength(ViewModelConstants.MaxNameLength, ErrorMessage = ViewModelConstants.StringLengthError, MinimumLength = ViewModelConstants.MinNameLength)]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        [DataType(DataType.Text)]
        [StringLength(ViewModelConstants.MaxNameLength, ErrorMessage = ViewModelConstants.StringLengthError, MinimumLength = ViewModelConstants.MinNameLength)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(ViewModelConstants.MaxBiographyLength, ErrorMessage = ViewModelConstants.StringLengthError, MinimumLength = ViewModelConstants.MinBiographyLength)]
        public string Biography { get; set; }
    }
}
