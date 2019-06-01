namespace Honeypot.Models
{
    public class Rating
    {
        public string UserId { get; set; }
        public HoneypotUser User { get; set; }

        public int Stars { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
