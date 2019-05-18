using Honeypot.Data.EntityConfiguration;
using Honeypot.Models;
using Honeypot.Models.MappingModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Honeypot.Data
{
    public class HoneypotDbContext : IdentityDbContext<HoneypotUser>
    {
        public HoneypotDbContext(DbContextOptions<HoneypotDbContext> options)
            : base(options)
        {
        }

        public DbSet<Rating> Ratings { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Quote> Quotes { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Bookshelf> Bookshelves { get; set; }

        public DbSet<UsersQuotes> UsersQuotes { get; set; }

        public DbSet<BooksBookshelves> BooksBookshelves { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<BooksBookshelves>()
                .HasKey(x => new {x.BookId, x.BookshelfId});

            builder.Entity<UsersQuotes>()
                .HasKey(x => new {x.QuoteId, x.UserId});

            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new AuthorConfiguration());
            builder.ApplyConfiguration(new BookConfiguration());
            builder.ApplyConfiguration(new QuoteConfiguration());

            base.OnModelCreating(builder);
        }
    }
}