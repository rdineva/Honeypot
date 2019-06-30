using System.Linq;
using Honeypot.Models;
using Honeypot.Models.MappingModels;
using Honeypot.Services.Contracts;
using Honeypot.Tests.Account;
using Xunit;

namespace Honeypot.Tests.Tests
{
    public class QuoteServiceTests : BaseTest
    {
        private readonly IQuoteService quoteService;

        public QuoteServiceTests(BaseTestFixture fixture) : base(fixture)
        {
            this.quoteService = fixture.Provider.GetService(typeof(IQuoteService)) as IQuoteService;
            this.SeedData();
        }

        private void SeedData()
        {
            this.DeleteQuotesData();
            this.DeleteUsersData();
            this.DeleteBooksData();
            this.DeleteAuthorsData();

            var author = new Author()
            {
                FirstName = TestsConstants.FirstName,
                LastName = TestsConstants.LastName,
                Id = TestsConstants.Id1
            };

            this.context.Authors.Add(author);

            var book = new Book()
            {
                Id = TestsConstants.Id1,
                Author = author,
                Title = TestsConstants.Title1,
            };

            this.context.Books.Add(book);

            var quote = new Quote()
            {
                Id = TestsConstants.Id1,
                Text = TestsConstants.Text,
                Book = book,
                Author = author
            };

            this.context.Quotes.Add(quote);
            var user = new HoneypotUser()
            {
                UserName = TestsConstants.Username,
                Id = TestsConstants.UserId
            };

            this.context.Users.Add(user);
            var userQuote = new UserQuote()
            {
                UserId = user.Id,
                QuoteId = quote.Id
            };

            user.LikedQuotes.Add(userQuote);
            this.context.UsersQuotes.Add(userQuote);
            this.context.SaveChanges();
        }

        [Fact]
        public void HasUserLikedQuote_ShouldReturnTrue_WhenUserHasLikedQuote()
        {
            var resultFromService = this.quoteService.HasUserLikedQuote(TestsConstants.Id1, TestsConstants.UserId);
            Assert.True(resultFromService);
        }

        [Fact]
        public void HasUserLikedQuote_ShouldReturnFalse_WhenUserHasNotLikedQuote()
        {
            var resultFromService = this.quoteService.HasUserLikedQuote(TestsConstants.Id2, TestsConstants.UserId);
            Assert.False(resultFromService);
        }

        [Fact]
        public void GetLikedQuotesByUser_ShouldReturAllLikedByUserQuotes()
        {
            var user = this.context.Users.FirstOrDefault(x => x.Id == TestsConstants.UserId);
            var resultFromService = this.quoteService.GetLikedQuotesByUser(user).ToList();
            var correctResult = this.context.UsersQuotes.Where(x => x.User == user).ToList().ConvertAll(x => x.Quote);
            Assert.Equal(correctResult, resultFromService);
        }

        [Fact]
        public void QuoteExists_ShouldReturnTrue_WhenQuoteTextExists()
        {
            var resultFromService = this.quoteService.QuoteExists(TestsConstants.Text);
            Assert.True(resultFromService);
        }

        [Fact]
        public void QuoteExists_ShouldReturnFalse_WhenQuoteTextDoesNotExist()
        {
            var resultFromService = this.quoteService.QuoteExists(TestsConstants.NonExistentText);
            Assert.False(resultFromService);
        }

        [Fact]
        public void GetQuoteById_ShouldReturnCorrectQuote()
        {
            var resultFromService = this.quoteService.GetQuoteById(TestsConstants.Id1);
            var correctResult = this.context.Quotes.FirstOrDefault(x => x.Id == TestsConstants.Id1);
            Assert.Equal(correctResult, resultFromService);
        }

        [Fact]
        public void GetUsersLikedQuoteById_ShouldReturnCorrectUserQuote()
        {
            var resultFromService = this.quoteService.GetUsersLikedQuoteById(TestsConstants.Id1, TestsConstants.UserId);
            var correctResult = this.context.UsersQuotes.FirstOrDefault(x =>
                x.UserId == TestsConstants.UserId && x.QuoteId == TestsConstants.Id1);
            Assert.Equal(correctResult, resultFromService);
        }
    }
}