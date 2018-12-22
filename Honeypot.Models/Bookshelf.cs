using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Honeypot.Models.MappingModels;

namespace Honeypot.Models
{
    public class Bookshelf
    {
        public Bookshelf()
        {
            this.Books = new List<BooksBookshelves>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public virtual ICollection<BooksBookshelves> Books { get; set; }

        [ForeignKey("Owner")]
        public string OwnerId { get; set; }
        public virtual HoneypotUser Owner { get; set; }
    }
}
