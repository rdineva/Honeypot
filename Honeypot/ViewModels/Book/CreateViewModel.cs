using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Honeypot.ViewModels.Book
{
    public class CreateViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        [StringLength(50, ErrorMessage = "Title is too long!")]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Summary { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [DisplayName("Author's First Name")]
        public string AuthorFirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [DisplayName("Author's Last Name")]
        public string AuthorLastName { get; set; }
    }
}