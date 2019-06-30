using System.Linq;
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

            var author = CreateAuthorData();
            var book = this.CreateBookData(author);
            var user = CreateUserData();
            var bookshelf = this.CreateBookshelfData(user);
            this.CreateBookBookshelfData(book, bookshelf);
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