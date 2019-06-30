using Honeypot.Data;
using Honeypot.Models;
using System.Collections.Generic;
using System.Linq;
using Honeypot.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Honeypot.Services
{
    public class BookshelfService : BaseService, IBookshelfService
    {
        public BookshelfService(HoneypotDbContext context) 
            : base(context)
        {
        }

        public List<Bookshelf> GetUsersBookshelves(string username)
        {
            var usersBookshelves = this.context
                .Bookshelves
                .Include(x => x.Books)
                .Include(x => x.User)
                .Where(x => x.User.UserName == username)
                .ToList();

            return usersBookshelves;
        }

        public bool IsBookInBookshelf(int bookId, int bookshelfId)
        {
            var isBookInBookshelf = this.context
                .BooksBookshelves
                .Any(x => x.BookId == bookId 
                       && x.BookshelfId == bookshelfId);

            return isBookInBookshelf;
        }

        public bool UserHasBookshelfTitled(string bookshelfName, string userId)
        {
            var userHasBookshelf = this.context
                .Bookshelves
                .Any(x => x.Title == bookshelfName 
                       && x.UserId == userId);

            return userHasBookshelf;
        }

        public Bookshelf GetUserBookshelfById(int bookshelfId, string userId)
        {
            var bookshelfResult = this.context
                .Bookshelves
                .Include(x => x.User)
                .Include(x => x.Books)
                .ThenInclude(x => x.Book)
                .ThenInclude(x => x.Author)
                .FirstOrDefaultAsync(x => x.Id == bookshelfId 
                                       && x.UserId == userId).Result;

            return bookshelfResult;
        }
    }
}