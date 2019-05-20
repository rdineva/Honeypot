using System.ComponentModel.DataAnnotations;

namespace Honeypot.ViewModels.Bookshelf
{
    public class CreateViewModel
    {
        //TODO: extract all error messages into common constants class
        [Required]
        [DataType(DataType.Text)]
        [StringLength(30, ErrorMessage = "Title must be between 3 and 30 characters!", MinimumLength = 3)]
        public string Title { get; set; }
    }
}
