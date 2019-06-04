using System.Collections.Generic;
using Honeypot.Models;

namespace Honeypot.Services.Contracts
{
    public interface IAuthorService
    {
        bool AuthorExists(string firstName, string lastName);

        Author GeAuthorById(int id);

        List<Author> GetAllAuthors();
    }
}
