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
                FirstName = TestConstants.FirstName,
                LastName = TestConstants.LastName,
                Id = TestConstants.IntegerId1
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
            var authorExists = this.authorService.AuthorExists(TestConstants.FirstName, TestConstants.LastName);
            Assert.True(authorExists);
        }

        [Fact]
        public void AuthorExists_ShouldReturnFalse_WhenAuthorDoesntExists()
        {
            var authorExists = this.authorService.AuthorExists(TestConstants.FirstNameNonExistent, TestConstants.LastNameNonExistent);
            Assert.False(authorExists);
        }

        [Fact]
        public void GetAuthorById_ShouldReturnAuthor()
        {
            var author = this.authorService.GetAuthorById(TestConstants.IntegerId1);
            Assert.Equal(author.Id, TestConstants.IntegerId1);
        }

        [Fact]
        public void GetAllAuthors_ShouldReturnAllAuthors()
        {
            var authorsFromService = this.authorService.GetAllAuthors();
            var correctAuthors = this.context.Authors.ToList();
            Assert.Equal(authorsFromService, correctAuthors);
        }

        [Fact]
        public void GetAuthorByName_ShouldReturnAuthor()
        {
            var authorFromService =
                this.authorService.GetAuthorByName(TestConstants.FirstName, TestConstants.LastName);
            var correctAuthor = this.context.Authors.FirstOrDefault(x =>
                x.FirstName == TestConstants.FirstName && x.LastName == TestConstants.LastName);

            Assert.Equal(authorFromService, correctAuthor);
        }
    }
}