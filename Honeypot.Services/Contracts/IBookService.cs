using System.Collections.Generic;
using Honeypot.Models;
using Honeypot.Models.Enums;

namespace Honeypot.Services.Contracts
{
    public interface IBookService
    {
        Book GetBookById(int id);

        bool BookTitleExists(string title, string authorFirstName, string authorLastName);

        List<Book> GetAllBooks();

        Genre[] GetAllGenres();

        List<Book> GetAllBooksByGenre(Genre genre);

        Dictionary<Genre, List<Book>> GetAllBooksFromAllGenres(Genre[] genres);
    }
}
