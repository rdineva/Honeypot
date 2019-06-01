using Honeypot.Models.Enums;

namespace Honeypot.Models
{
    public class Rating
    {
        public string UserId { get; set; }
        public HoneypotUser User { get; set; }

        public StarRating Stars { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
