using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Honeypot.ViewModels.Review
{
    public class CreateViewModel
    {
        [DataType(DataType.Text)]
        [DisplayName("Review")]
        public string Text { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [DisplayName("Book Title")]
        public string BookTitle { get; set; }

        [Required]
        public int Rating { get; set; }
    }
}
