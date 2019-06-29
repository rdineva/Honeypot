using System.Collections.Generic;
using Honeypot.Models;

namespace Honeypot.Services.Contracts
{
    public interface IAuthorService
    {
        bool AuthorExists(string firstName, string lastName);

        Author GetAuthorById(int id);

        List<Author> GetAllAuthors();

        Author GetAuthorByName(string firstName, string lastName);
    }
}
