using System.Linq;
using Honeypot.Data;
using Honeypot.Models;
using Honeypot.Models.Enums;
using Honeypot.Services.Contracts;
using Honeypot.Tests.Account;
using Xunit;

namespace Honeypot.Tests.Tests
{
    public class RatingServiceTests : IClassFixture<BaseTest>
    {
        private readonly IRatingService ratingService;

        private readonly HoneypotDbContext context;

        public RatingServiceTests(BaseTest fixture)
        {
            this.ratingService = fixture.Provider.GetService(typeof(IRatingService)) as IRatingService;
            this.context = fixture.Provider.GetService(typeof(HoneypotDbContext)) as HoneypotDbContext;
            this.SeedData();
        }

        private void SeedData()
        {
            this.DeleteUsersData();
            this.DeleteAuthorsData();
            this.DeleteBooksData();
            this.DeleteRatingsData();

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
                Genre = Genre.Adventure
            };

            this.context.Books.Add(book);
            var user = new HoneypotUser()
            {
                UserName = TestsConstants.Username,
                Id = TestsConstants.UserId
            };

            this.context.Users.Add(user);
            Rating rating = new Rating()
            {
                UserId = user.Id,
                BookId = book.Id,
                Stars = StarRating.Awesome
            };

            this.context.Ratings.Add(rating);
            this.context.SaveChanges();
        }

        private void DeleteRatingsData()
        {
            var ratings = this.context.Ratings.ToList();
            this.context.Ratings.RemoveRange(ratings);
            this.context.SaveChanges();
        }

        private void DeleteUsersData()
        {
            var users = this.context.Users.ToList();
            this.context.Users.RemoveRange(users);
            this.context.SaveChanges();
        }

        private void DeleteBooksData()
        {
            var books = this.context.Books.ToList();
            this.context.Books.RemoveRange(books);
            this.context.SaveChanges();
        }

        private void DeleteAuthorsData()
        {
            var authors = this.context.Authors.ToList();
            this.context.Authors.RemoveRange(authors);
            this.context.SaveChanges();
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