using System.Collections.Generic;
using System.Linq;
using Honeypot.Data;
using Honeypot.Models;
using Honeypot.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Honeypot.Services
{
    public class AuthorService : BaseService, IAuthorService
    {
        public AuthorService(HoneypotDbContext context)
            : base(context)
        {
        }

        public bool AuthorExists(string firstName, string lastName)
        {
            var authorExists = this.context
                .Authors
                .Any(x => x.FirstName == firstName 
                       && x.LastName == lastName);

            return authorExists;
        }

        public Author GetAuthorById(int id)
        {
            var author = this.context
                .Authors
                .Include(x => x.Books)
                .ThenInclude(x => x.Quotes)
                .FirstOrDefault(x => x.Id == id);

            return author;
        }

        public List<Author> GetAllAuthors()
        {
            var authors = this.context.Authors.ToList();
            return authors;
        }

        public Author GetAuthorByName(string firstName, string lastName)
        {
            var author = this.context
                .Authors
                .FirstOrDefault(x => x.FirstName == firstName
                          && x.LastName == lastName);

            return author;
        }
    }
}
