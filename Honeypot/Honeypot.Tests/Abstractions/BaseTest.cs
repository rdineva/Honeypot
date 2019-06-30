using System.Linq;
using Honeypot.Data;
using Honeypot.Models;
using Honeypot.Models.Enums;
using Honeypot.Models.MappingModels;
using Honeypot.Tests.Account;
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

        protected Author CreateAuthorData()
        {
            var author = new Author()
            {
                FirstName = TestsConstants.FirstName,
                LastName = TestsConstants.LastName,
                Id = TestsConstants.Id1
            };

            this.context.Authors.Add(author);
            this.context.SaveChanges();
            return author;
        }

        protected Book CreateBookData(Author author)
        {
            var book = new Book()
            {
                Id = TestsConstants.Id1,
                Author = author,
                Title = TestsConstants.Title1,
                Genre = Genre.Adventure
            };

            this.context.Books.Add(book);
            this.context.SaveChanges();
            return book;
        }

        protected HoneypotUser CreateUserData()
        {
            var user = new HoneypotUser()
            {
                UserName = TestsConstants.Username,
                Id = TestsConstants.UserId
            };

            this.context.Users.Add(user);
            this.context.SaveChanges();
            return user;
        }

        protected Bookshelf CreateBookshelfData(HoneypotUser user)
        {
            var bookshelf = new Bookshelf()
            {
                Title = TestsConstants.Title1,
                UserId = user.Id,
                Id = TestsConstants.Id2
            };

            this.context.Bookshelves.Add(bookshelf);
            this.context.SaveChanges();
            return bookshelf;
        }

        protected BookBookshelf CreateBookBookshelfData(Book book, Bookshelf bookshelf)
        {
            var bookBookshelf = new BookBookshelf()
            {
                Book = book,
                Bookshelf = bookshelf
            };

            this.context.BooksBookshelves.Add(bookBookshelf);
            this.context.SaveChanges();
            return bookBookshelf;
        }

        protected Quote CreateQuoteData(Book book, Author author)
        {
            var quote = new Quote()
            {
                Id = TestsConstants.Id1,
                Text = TestsConstants.Text,
                Book = book,
                Author = author
            };

            this.context.Quotes.Add(quote);
            this.context.SaveChanges();
            return quote;
        }

        protected UserQuote CreateUserQuoteData(HoneypotUser user, Quote quote)
        {
            var userQuote = new UserQuote()
            {
                UserId = user.Id,
                QuoteId = quote.Id
            };

            user.LikedQuotes.Add(userQuote);
            this.context.UsersQuotes.Add(userQuote);
            this.context.SaveChanges();
            return userQuote;
        }

        protected Rating CreateRatingData(HoneypotUser user, Book book)
        {
            Rating rating = new Rating()
            {
                UserId = user.Id,
                BookId = book.Id,
                Stars = StarRating.Awesome
            };

            this.context.Ratings.Add(rating);
            this.context.SaveChanges();
            return rating;
        }
    }
}