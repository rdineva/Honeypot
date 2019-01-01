using Honeypot.Data;
using Honeypot.Models;
using System.Collections.Generic;
using System.Linq;

namespace Honeypot.Services
{
    public class HoneypotUsersBookshelvesService : BaseService
    {
        public HoneypotUsersBookshelvesService(HoneypotDbContext context) : base(context) { }

        public List<Bookshelf> GetUsersBookshelves(string userId) => this.context.Bookshelves.Where(x => x.OwnerId == userId).ToList();

        public bool CheckIfBookIsInBookshelf(int bookId, int shelfId) => this.context.Bookshelves.Where(x => x.Id == shelfId).Any(x => x.Id == bookId);
    }
}
