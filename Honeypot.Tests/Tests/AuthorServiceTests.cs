using System.Linq;
using Honeypot.Data;
using Honeypot.Models;
using Honeypot.Services.Contracts;
using Honeypot.Tests.Account;
using Xunit;

namespace Honeypot.Tests
{
    public class AuthorServiceTests : IClassFixture<BaseTest>
    {
        private readonly IAuthorService authorService;

        private readonly HoneypotDbContext context;

        public AuthorServiceTests(BaseTest fixture)
        {
            this.authorService = fixture.Provider.GetService(typeof(IAuthorService)) as IAuthorService;
            this.context = fixture.Provider.GetService(typeof(HoneypotDbContext)) as HoneypotDbContext;
            this.SeedData();
        }

        private void SeedData()
        {
            this.DeleteAuthorsData();
            var author = new Author()
            {
                FirstName = TestsConstants.FirstName,
                LastName = TestsConstants.LastName,
                Id = TestsConstants.Id1
            };

            this.context.Authors.Add(author);
            this.context.SaveChanges();
        }

        private void DeleteAuthorsData()
        {
            var authors = this.context.Authors.ToList();
            this.context.Authors.RemoveRange(authors);
            this.context.SaveChanges();
        }

        [Fact]
        public void AuthorExists_ShouldReturnTrue_WhenAuthorExists()
        {
            var resultFromService = this.authorService.AuthorExists(TestsConstants.FirstName, TestsConstants.LastName);
            Assert.True(resultFromService);
        }

        [Fact]
        public void AuthorExists_ShouldReturnFalse_WhenAuthorDoesntExists()
        {
            var resultFromService = this.authorService.AuthorExists(TestsConstants.FirstNameNonExistent, TestsConstants.LastNameNonExistent);
            Assert.False(resultFromService);
        }

        [Fact]
        public void GetAuthorById_ShouldReturnAuthor()
        {
            var author = this.authorService.GetAuthorById(TestsConstants.Id1);
            Assert.Equal(TestsConstants.Id1, author.Id);
        }

        [Fact]
        public void GetAllAuthors_ShouldReturnAllAuthors()
        {
            var authorsFromService = this.authorService.GetAllAuthors();
            var correctAuthors = this.context.Authors.ToList();
            Assert.Equal(correctAuthors, authorsFromService);
        }

        [Fact]
        public void GetAuthorByName_ShouldReturnAuthor()
        {
            var authorFromService =
                this.authorService.GetAuthorByName(TestsConstants.FirstName, TestsConstants.LastName);
            var correctAuthor = this.context.Authors.FirstOrDefault(x =>
                x.FirstName == TestsConstants.FirstName && x.LastName == TestsConstants.LastName);

            Assert.Equal(correctAuthor, authorFromService);
        }
    }
}