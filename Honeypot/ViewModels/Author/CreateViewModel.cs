using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Honeypot.ViewModels.Author
{
    public class CreateViewModel
    {
        [Required]
        [DisplayName("First Name")]
        [DataType(DataType.Text)]
        [StringLength(25, ErrorMessage = "First name is too long!", MinimumLength = 3)]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        [DataType(DataType.Text)]
        [StringLength(25, ErrorMessage = "Last name is too long!", MinimumLength = 3)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(500, ErrorMessage = "Biography is too long!", MinimumLength = 10)]
        public string Biography { get; set; }
    }
}
