using System.Linq;
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

            var author = this.CreateAuthorData();
            var book = this.CreateBookData(author);
            var quote = this.CreateQuoteData(book, author);
            var user = this.CreateUserData();
            this.CreateUserQuoteData(user, quote);
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