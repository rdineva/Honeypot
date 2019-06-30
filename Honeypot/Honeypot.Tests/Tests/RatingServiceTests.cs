using System.Linq;
using Honeypot.Services.Contracts;
using Honeypot.Tests.Account;
using Xunit;

namespace Honeypot.Tests.Tests
{
    public class RatingServiceTests : BaseTest
    {
        private readonly IRatingService ratingService;

        public RatingServiceTests(BaseTestFixture fixture) : base(fixture)
        {
            this.ratingService = fixture.Provider.GetService(typeof(IRatingService)) as IRatingService;
            this.SeedData();
        }

        private void SeedData()
        {
            this.DeleteUsersData();
            this.DeleteAuthorsData();
            this.DeleteBooksData();
            this.DeleteRatingsData();

            var author = this.CreateAuthorData();
            var book = this.CreateBookData(author);
            var user = this.CreateUserData();
            this.CreateRatingData(user, book);
        }

        [Fact]
        public void HasUserRatedBook_ShouldReturnTrue_WhenUserHasRatedBook()
        {
            var resultFromService = this.ratingService.HasUserRatedBook(TestsConstants.UserId, TestsConstants.Id1);
            Assert.True(resultFromService);
        }

        [Fact]
        public void HasUserRatedBook_ShouldReturnFalse_WhenUserHasNotRatedBook()
        {
            var resultFromService = this.ratingService.HasUserRatedBook(TestsConstants.UserId, TestsConstants.Id2);
            Assert.False(resultFromService);
        }

        [Fact]
        public void GetUserBookRating_ShouldReturnCorrectUserBookRating()
        {
            var resultFromService = this.ratingService.GetUserBookRating(TestsConstants.UserId, TestsConstants.Id1);
            var correctResult = this.context.Ratings.FirstOrDefault(x =>
                x.UserId == TestsConstants.UserId && x.BookId == TestsConstants.Id1);
            Assert.Equal(correctResult, resultFromService);
        }
    }
}