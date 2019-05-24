using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Honeypot.Attributes;

namespace Honeypot.ViewModels.Account
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(25, ErrorMessage = "Username can be between 5 and 25 symbols!", MinimumLength = 5)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(int.MaxValue, ErrorMessage = "Password must be at least 6 symbols", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DisplayName("Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The passwords don't match.")]
        public string ConfirmPassword { get; set; }

        [NotNullOrWhiteSpace]
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [NotNullOrWhiteSpace]
        [DisplayName("Last Name")]
        public string LastName { get; set; }
    }
}
