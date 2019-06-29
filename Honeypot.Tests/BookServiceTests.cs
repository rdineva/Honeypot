using System.Linq;
using Honeypot.Data;
using Honeypot.Models;
using Honeypot.Models.Enums;
using Honeypot.Services.Contracts;
using Honeypot.Tests.Account;
using Xunit;

namespace Honeypot.Tests
{
    public class BookServiceTests : IClassFixture<BaseTest>
    {
        private readonly IBookService bookService;

        private readonly HoneypotDbContext context;

        public BookServiceTests(BaseTest fixture)
        {
            this.bookService = fixture.Provider.GetService(typeof(IBookService)) as IBookService;
            this.context = fixture.Provider.GetService(typeof(HoneypotDbContext)) as HoneypotDbContext;
            this.SeedData();
        }

        private void SeedData()
        {
            this.DeleteBooksData();
            this.DeleteAuthorsData();
            var author = new Author()
            {
                FirstName = TestConstants.FirstName,
                LastName = TestConstants.LastName,
                Id = TestConstants.IntegerId1
            };

            this.context.Authors.Add(author);

            var book1 = new Book()
            {
                Id = TestConstants.IntegerId1,
                Author = author,
                Title = TestConstants.Title1,
                Genre = Genre.Adventure
            };

            var book2 = new Book()
            {
                Id = TestConstants.IntegerId2,
                Author = author,
                Title = TestConstants.Title2,
                Genre = Genre.Adventure
            };

            this.context.Books.Add(book1);
            this.context.Books.Add(book2);
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
        public void GetBookById_ShouldReturnBook()
        {
            var book = this.bookService.GetBookById(TestConstants.IntegerId1);
            Assert.Equal(book.Id, TestConstants.IntegerId1);
        }

        [Fact]
        public void BookTitleExists_ShouldReturnTrue_WhenBookTitleFromAuthorExists()
        {
            var bookTitleExists = this.bookService.BookTitleExists(TestConstants.Title1, TestConstants.FirstName,
                TestConstants.LastName);
            Assert.True(bookTitleExists);
        }

        [Fact]
        public void BookTitleExists_ShouldReturnFalse_WhenBookTitleExistsButFromAnotherAuthor()
        {
            var bookTitleExists = this.bookService.BookTitleExists(TestConstants.Title1, TestConstants.FirstNameNonExistent,
                TestConstants.LastNameNonExistent);
            Assert.False(bookTitleExists);
        }

        [Fact]
        public void BookTitleExists_ShouldReturnFalse_WhenBookTitleFromAuthorDoesntExists()
        {
            var bookTitleExists = this.bookService.BookTitleExists(TestConstants.Title2, TestConstants.FirstNameNonExistent,
                TestConstants.LastNameNonExistent);
            Assert.False(bookTitleExists);
        }

        [Fact]
        public void GetAllBooks_ShouldReturnAllBooks()
        {
            var allBooksFromService = this.bookService.GetAllBooks();
            var correctResult = this.context.Books.ToList();
            Assert.Equal(allBooksFromService, correctResult);
        }
    }
}
