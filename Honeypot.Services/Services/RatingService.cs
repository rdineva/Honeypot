using System.Linq;
using Honeypot.Data;
using Honeypot.Models;
using Honeypot.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Honeypot.Services
{
    public class RatingService : BaseService, IRatingService
    {
        public RatingService(HoneypotDbContext context) : base(context)
        {
        }

        public bool HasUserRatedBook(string userId, int bookId)
        {
            var userHasRatedBook = this.context
                .Ratings
                .Include(x => x.Book)
                .Include(x => x.User)
                .Any(x => x.BookId == bookId
                          && x.UserId == userId);

            return userHasRatedBook;
        }

        public Rating FindUserBookRating(string userId, int bookId)
        {
            var rating = this.context
                .Ratings
                .Include(x => x.Book)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.BookId == bookId
                                          && x.UserId == userId).Result;

            return rating;
        }
    }
}
