using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Honeypot.ViewModels.Author
{
    public class CreateViewModel
    {
        [Required]
        [DisplayName("First Name")]
        [DataType(DataType.Text)]
        [StringLength(25, ErrorMessage = "First name is too long")]
        public string FirstName { get; set; }

        [Required]
        [DisplayName("Last Name")]
        [DataType(DataType.Text)]
        [StringLength(25, ErrorMessage = "Last name is too long")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(500, ErrorMessage = "Biography is too long")]
        public string Biography { get; set; }
    }
}
