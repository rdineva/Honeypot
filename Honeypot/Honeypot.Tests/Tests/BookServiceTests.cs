using System;
using System.Linq;
using Honeypot.Models;
using Honeypot.Models.Enums;
using Honeypot.Services.Contracts;
using Honeypot.Tests.Account;
using Xunit;
using System.Collections.Generic;
using Honeypot.Tests.Tests;

namespace Honeypot.Tests
{
    public class BookServiceTests : BaseTest
    {
        private readonly IBookService bookService;

        public BookServiceTests(BaseTestFixture fixture) : base(fixture)
        {
            this.bookService = fixture.Provider.GetService(typeof(IBookService)) as IBookService;
            this.SeedData();
        }

        private void SeedData()
        {
            this.DeleteBooksData();
            this.DeleteAuthorsData();
            var author = new Author()
            {
                FirstName = TestsConstants.FirstName,
                LastName = TestsConstants.LastName,
                Id = TestsConstants.Id1
            };

            this.context.Authors.Add(author);

            var book1 = new Book()
            {
                Id = TestsConstants.Id1,
                Author = author,
                Title = TestsConstants.Title1,
                Genre = Genre.Adventure
            };

            var book2 = new Book()
            {
                Id = TestsConstants.Id2,
                Author = author,
                Title = TestsConstants.Title2,
                Genre = Genre.Adventure
            };

            this.context.Books.Add(book1);
            this.context.Books.Add(book2);
            this.context.SaveChanges();
        }

        [Fact]
        public void GetBookById_ShouldReturnBook()
        {
            var book = this.bookService.GetBookById(TestsConstants.Id1);
            Assert.Equal(TestsConstants.Id1, book.Id);
        }

        [Fact]
        public void BookTitleExists_ShouldReturnTrue_WhenBookTitleFromAuthorExists()
        {
            var resultFromService = this.bookService.BookTitleExists(TestsConstants.Title1, TestsConstants.FirstName,
                TestsConstants.LastName);
            Assert.True(resultFromService);
        }

        [Fact]
        public void BookTitleExists_ShouldReturnFalse_WhenBookTitleExistsButFromAnotherAuthor()
        {
            var resultFromService = this.bookService.BookTitleExists(TestsConstants.Title1, TestsConstants.FirstNameNonExistent,
                TestsConstants.LastNameNonExistent);
            Assert.False(resultFromService);
        }

        [Fact]
        public void BookTitleExists_ShouldReturnFalse_WhenBookTitleFromAuthorDoesntExists()
        {
            var resultFromService = this.bookService.BookTitleExists(TestsConstants.Title2, TestsConstants.FirstNameNonExistent,
                TestsConstants.LastNameNonExistent);
            Assert.False(resultFromService);
        }

        [Fact]
        public void GetAllBooks_ShouldReturnAllBooks()
        {
            var resultFromService = this.bookService.GetAllBooks();
            var correctResult = this.context.Books.ToList();
            Assert.Equal(correctResult, resultFromService);
        }

        [Fact]
        public void GetAllGenres_ShouldReturnAllGenres()
        {
            var resultFromService = this.bookService.GetAllGenres();
            var correctResult = (Genre[])Enum.GetValues(typeof(Genre));
            Assert.Equal(correctResult, resultFromService);
        }

        [Fact]
        public void GetAllBooksByGenre_ShouldReturnAllBooksByGenre()
        {
            var resultFromService = this.bookService.GetAllBooksByGenre(Genre.Adventure);
            var correctResult = this.context.Books.Where(x => x.Genre == Genre.Adventure).ToList();
            Assert.Equal(correctResult, resultFromService);
        }

        [Fact]
        public void GetAllBooksByGenre_ShouldReturnNull_WhenNoBooksInDbFromOneGenre()
        {
            var resultFromService = this.bookService.GetAllBooksByGenre(Genre.Romance);
            Assert.Empty(resultFromService);
        }

        [Fact]
        public void GetAllBooksFromAllGenres_ShouldReturnAllBooksFromAllGenres()
        {
            var genres = (Genre[])Enum.GetValues(typeof(Genre));
            var resultFromService = this.bookService.GetAllBooksFromAllGenres(genres);
            var correctResults = new Dictionary<Genre, List<Book>>();
            foreach (var genre in genres)
            {
                correctResults[genre] = new List<Book>();
            }

            correctResults[Genre.Adventure].AddRange(this.context.Books.Where(x => x.Genre == Genre.Adventure).ToList());
            Assert.Equal(correctResults, resultFromService);
        }
    }
}