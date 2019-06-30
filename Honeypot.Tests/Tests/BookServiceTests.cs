using System;
using System.Linq;
using Honeypot.Data;
using Honeypot.Models;
using Honeypot.Models.Enums;
using Honeypot.Services.Contracts;
using Honeypot.Tests.Account;
using Xunit;
using System.Collections.Generic;

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
                FirstName = TestsConstants.FirstName,
                LastName = TestsConstants.LastName,
                Id = TestsConstants.IntegerId1
            };

            this.context.Authors.Add(author);

            var book1 = new Book()
            {
                Id = TestsConstants.IntegerId1,
                Author = author,
                Title = TestsConstants.Title1,
                Genre = Genre.Adventure
            };

            var book2 = new Book()
            {
                Id = TestsConstants.IntegerId2,
                Author = author,
                Title = TestsConstants.Title2,
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
            var book = this.bookService.GetBookById(TestsConstants.IntegerId1);
            Assert.Equal(book.Id, TestsConstants.IntegerId1);
        }

        [Fact]
        public void BookTitleExists_ShouldReturnTrue_WhenBookTitleFromAuthorExists()
        {
            var bookTitleExists = this.bookService.BookTitleExists(TestsConstants.Title1, TestsConstants.FirstName,
                TestsConstants.LastName);
            Assert.True(bookTitleExists);
        }

        [Fact]
        public void BookTitleExists_ShouldReturnFalse_WhenBookTitleExistsButFromAnotherAuthor()
        {
            var bookTitleExists = this.bookService.BookTitleExists(TestsConstants.Title1, TestsConstants.FirstNameNonExistent,
                TestsConstants.LastNameNonExistent);
            Assert.False(bookTitleExists);
        }

        [Fact]
        public void BookTitleExists_ShouldReturnFalse_WhenBookTitleFromAuthorDoesntExists()
        {
            var bookTitleExists = this.bookService.BookTitleExists(TestsConstants.Title2, TestsConstants.FirstNameNonExistent,
                TestsConstants.LastNameNonExistent);
            Assert.False(bookTitleExists);
        }

        [Fact]
        public void GetAllBooks_ShouldReturnAllBooks()
        {
            var allBooksFromService = this.bookService.GetAllBooks();
            var correctResult = this.context.Books.ToList();
            Assert.Equal(allBooksFromService, correctResult);
        }

        [Fact]
        public void GetAllGenres_ShouldReturnAllGenres()
        {
            var allGenresFromService = this.bookService.GetAllGenres();
            var correctResult = (Genre[])Enum.GetValues(typeof(Genre));
            Assert.Equal(allGenresFromService, correctResult);
        }

        [Fact]
        public void GetAllBooksByGenre_ShouldReturnAllBooksByGenre()
        {
            var allBookByGenreFromService = this.bookService.GetAllBooksByGenre(Genre.Adventure);
            var correctResult = this.context.Books.Where(x => x.Genre == Genre.Adventure).ToList();
            Assert.Equal(allBookByGenreFromService, correctResult);
        }

        [Fact]
        public void GetAllBooksByGenre_ShouldReturnNull_WhenNoBooksInDbFromOneGenre()
        {
            var allBookByGenreFromService = this.bookService.GetAllBooksByGenre(Genre.Romance);
            Assert.Empty(allBookByGenreFromService);
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
            Assert.Equal(resultFromService, correctResults);
        }
    }
}