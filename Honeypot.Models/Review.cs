using Honeypot.Models.Enums;

namespace Honeypot.Models
{
    public class Review
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }

        public string OwnerId { get; set; }
        public HoneypotUser Owner { get; set; }

        public Rating Rating { get; set; }
    }
}
