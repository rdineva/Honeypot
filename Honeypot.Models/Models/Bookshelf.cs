using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Honeypot.Models.MappingModels;
using Honeypot.Models.Models;

namespace Honeypot.Models
{
    public class Bookshelf : BaseModel
    {
        public Bookshelf()
        {
            this.Books = new List<BookBookshelf>();
        }

        public string Title { get; private set; }

        public virtual ICollection<BookBookshelf> Books { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public HoneypotUser User { get; set; }
    }
}