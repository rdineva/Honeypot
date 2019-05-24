using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Honeypot.Attributes;

namespace Honeypot.ViewModels.Author
{
    public class CreateAuthorViewModel
    {
        [Required]
        [DisplayName("First Name")]
        [DataType(DataType.Text)]
        [StringLength(25, ErrorMessage = "First name should be between 3 and 25 characters!", MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        [DataType(DataType.Text)]
        [StringLength(25, ErrorMessage = "Last name should be between 3 and 25 characters!", MinimumLength = 3)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(500, ErrorMessage = "Biography is should be between 10 and 500 characters!", MinimumLength = 10)]
        public string Biography { get; set; }
    }
}
