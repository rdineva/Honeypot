using Honeypot.Models;

namespace Honeypot.Services.Contracts
{
    public interface IRatingService
    {
        bool HasUserRatedBook(string userId, int bookId);

        Rating GetUserBookRating(string userId, int bookId);
    }
}
