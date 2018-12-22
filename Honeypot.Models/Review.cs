using System.ComponentModel.DataAnnotations.Schema;
using Honeypot.Models.Enums;

namespace Honeypot.Models
{
    public class Review
    {
        public int Id { get; set; }

        public string Text { get; set; }

        [ForeignKey("Book")]
        public int BookId { get; set; }
        public virtual Book Book { get; set; }

        [ForeignKey("Owner")]
        public string OwnerId { get; set; }
        public virtual HoneypotUser Owner { get; set; }

        public Rating Rating { get; set; }
    }
}
