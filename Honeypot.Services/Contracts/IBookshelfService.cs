using System.Collections.Generic;
using Honeypot.Models;

namespace Honeypot.Services.Contracts
{
    public interface IBookshelfService
    {
        List<Bookshelf> GetUsersBookshelves(string userId);

        bool IsBookInBookshelf(int bookId, int bookshelfId);

        bool UserHasBookshelfTitled(string bookshelfName, string userId);

        Bookshelf GetUserBookshelfById(int bookshelfId, string userId);
    }
}
