using System.Linq;
using Honeypot.Models;
using Honeypot.Models.Enums;
using Honeypot.Models.MappingModels;
using Honeypot.Services.Contracts;
using Honeypot.Tests.Account;
using Xunit;

namespace Honeypot.Tests.Tests
{
    public class BookshelfServiceTests : BaseTest
    {
        private readonly IBookshelfService bookshelfService;

        public BookshelfServiceTests(BaseTestFixture fixture) : base(fixture)
        {
            this.bookshelfService = fixture.Provider.GetService(typeof(IBookshelfService)) as IBookshelfService;
            this.SeedData();
        }

        private void SeedData()
        {
            this.DeleteUsersData();
            this.DeleteAuthorsData();
            this.DeleteBooksData();
            this.DeleteBookshelvesData();

            var user = new HoneypotUser()
            {
                UserName = TestsConstants.Username,
                Id = TestsConstants.UserId
            };

            this.context.Users.Add(user);
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
            var bookshelf = new Bookshelf()
            {
                Title = TestsConstants.Title1,
                UserId = user.Id,
                Id = TestsConstants.Id2
            };

            var bookBookshelf = new BookBookshelf()
            {
                Book = book,
                Bookshelf = bookshelf
            };

            this.context.Bookshelves.Add(bookshelf);
            this.context.BooksBookshelves.Add(bookBookshelf);
            this.context.SaveChanges();
        }

        [Fact]
        public void GetUsersBookshelves_ShouldReturnUserBookshelves()
        {
            var resultFromService = this.bookshelfService.GetUsersBookshelves(TestsConstants.Username);
            var correctResult = this.context.Bookshelves.Where(x => x.UserId == TestsConstants.UserId).ToList();
            Assert.Equal(correctResult, resultFromService);
        }

        [Fact]
        public void IsBookInBookshelf_ShouldReturnTrue_WhenBookIsInBookshelf()
        {
            var resultFromService = this.bookshelfService.IsBookInBookshelf(TestsConstants.Id1, TestsConstants.Id2);
            Assert.True(resultFromService);
        }

        [Fact]
        public void IsBookInBookshelf_ShouldReturnFalse_WhenBookIsNotInBookshelf()
        {
            var resultFromService = this.bookshelfService.IsBookInBookshelf(TestsConstants.Id2, TestsConstants.Id2);
            Assert.False(resultFromService);
        }

        [Fact]
        public void UserHasBookshelfTitled_ShouldReturnTrue_WhenUserHasBookshelfWithThisTitle()
        {
            var resultFromService = this.bookshelfService.UserHasBookshelfTitled(TestsConstants.Title1, TestsConstants.UserId);
            Assert.True(resultFromService);
        }

        [Fact]
        public void UserHasBookshelfTitled_ShouldReturnFalse_WhenUserDoesntHaveBookshelfWithThisTitle()
        {
            var resultFromService = this.bookshelfService.UserHasBookshelfTitled(TestsConstants.Title2, TestsConstants.UserId);
            Assert.False(resultFromService);
        }

        [Fact]
        public void GetUserBookshelfById_ShouldReturnUserBookshelf()
        {
            var resultFromService =
                this.bookshelfService.GetUserBookshelfById(TestsConstants.Id2, TestsConstants.UserId);
            Assert.Equal(TestsConstants.Id2, resultFromService.Id);
        }
    }
}