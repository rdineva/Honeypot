using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Honeypot.Attributes;

namespace Honeypot.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(ErrorConstants.MaxPasswordLength, ErrorMessage = ErrorConstants.StringLengthError, MinimumLength = ErrorConstants.MinNameLength)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(ErrorConstants.MaxPasswordLength, ErrorMessage = ErrorConstants.StringLengthError, MinimumLength = ErrorConstants.MinNameLength)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DisplayName("Confirm Password")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = ErrorConstants.PasswordsDontMatch)]
        public string ConfirmPassword { get; set; }

        [NotNullOrWhiteSpace]
        [DisplayName("First Name")]
        [StringLength(ErrorConstants.MaxPasswordLength, ErrorMessage = ErrorConstants.StringLengthError, MinimumLength = ErrorConstants.MinNameLength)]
        public string FirstName { get; set; }

        [NotNullOrWhiteSpace]
        [DisplayName("Last Name")]
        [StringLength(ErrorConstants.MaxPasswordLength, ErrorMessage = ErrorConstants.StringLengthError, MinimumLength = ErrorConstants.MinNameLength)]
        public string LastName { get; set; }
    }
}
