using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Honeypot.ViewModels.Author
{
    public class CreateAuthorViewModel
    {
        [Required]
        [DisplayName("First Name")]
        [DataType(DataType.Text)]
        [StringLength(ErrorConstants.MaxNameLength, ErrorMessage = ErrorConstants.StringLengthError, MinimumLength = ErrorConstants.MinNameLength)]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        [DataType(DataType.Text)]
        [StringLength(ErrorConstants.MaxNameLength, ErrorMessage = ErrorConstants.StringLengthError, MinimumLength = ErrorConstants.MinNameLength)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(ErrorConstants.MaxBiographyLength, ErrorMessage = ErrorConstants.StringLengthError, MinimumLength = ErrorConstants.MinBiographyLength)]
        public string Biography { get; set; }
    }
}
