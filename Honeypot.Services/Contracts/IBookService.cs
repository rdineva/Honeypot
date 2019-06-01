using Honeypot.Models;

namespace Honeypot.Services.Contracts
{
    public interface IBookService
    {
        Book GeBookById(int id);

        bool BookTitleExists(string title, string authorFirstName, string authorLastName);

        bool HasUserRatedBook(string userId, int bookId);

        Rating FindUserBookRating(string userId, int bookId);
    }
}
