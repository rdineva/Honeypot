using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Honeypot.Attributes;

namespace Honeypot.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required]
        [UniqueUsername]
        [StringLength(ViewModelConstants.MaxPasswordLength, ErrorMessage = ViewModelConstants.StringLengthError, MinimumLength = ViewModelConstants.MinNameLength)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(ViewModelConstants.MaxPasswordLength, ErrorMessage = ViewModelConstants.StringLengthError, MinimumLength = ViewModelConstants.MinNameLength)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DisplayName("Confirm Password")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = ViewModelConstants.PasswordsDontMatch)]
        public string ConfirmPassword { get; set; }

        [NotNullOrWhiteSpace]
        [DisplayName("First Name")]
        [StringLength(ViewModelConstants.MaxPasswordLength, ErrorMessage = ViewModelConstants.StringLengthError, MinimumLength = ViewModelConstants.MinNameLength)]
        public string FirstName { get; set; }

        [NotNullOrWhiteSpace]
        [DisplayName("Last Name")]
        [StringLength(ViewModelConstants.MaxPasswordLength, ErrorMessage = ViewModelConstants.StringLengthError, MinimumLength = ViewModelConstants.MinNameLength)]
        public string LastName { get; set; }
    }
}
