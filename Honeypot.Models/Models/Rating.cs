using Honeypot.Models.Models;

namespace Honeypot.Models
{
    public class Rating : BaseModel
    {
        public string UserId { get; set; }
        public HoneypotUser User { get; set; }

        public int Stars { get; private set; }

        public int BookId { get; set; }
        public Book Book { get; set; }
    }
}
