using System.Linq;
using Honeypot.Data;
using Xunit;

namespace Honeypot.Tests.Tests
{
    public abstract class BaseTest : IClassFixture<BaseTestFixture>
    {
        protected readonly HoneypotDbContext context;

        protected BaseTest(BaseTestFixture fixture)
        {
            this.context = fixture.Provider.GetService(typeof(HoneypotDbContext)) as HoneypotDbContext;
        }

        protected void DeleteAuthorsData()
        {
            var authors = this.context.Authors.ToList();
            this.context.Authors.RemoveRange(authors);
            this.context.SaveChanges();
        }

        protected void DeleteUsersData()
        {
            var users = this.context.Users.ToList();
            this.context.Users.RemoveRange(users);
            this.context.SaveChanges();
        }

        protected void DeleteBookshelvesData()
        {
            var bookshelves = this.context.Bookshelves.ToList();
            this.context.Bookshelves.RemoveRange(bookshelves);
            this.context.SaveChanges();
        }

        protected void DeleteBooksData()
        {
            var books = this.context.Books.ToList();
            this.context.Books.RemoveRange(books);
            this.context.SaveChanges();
        }

        protected void DeleteQuotesData()
        {
            var quotes = this.context.Quotes.ToList();
            this.context.Quotes.RemoveRange(quotes);
            this.context.SaveChanges();
        }

        protected void DeleteRatingsData()
        {
            var ratings = this.context.Ratings.ToList();
            this.context.Ratings.RemoveRange(ratings);
            this.context.SaveChanges();
        }
    }
}