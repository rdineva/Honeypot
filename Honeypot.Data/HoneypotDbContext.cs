using Honeypot.Models;
using Honeypot.Models.MappingModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Honeypot.Data
{
    public class HoneypotDbContext : IdentityDbContext<HoneypotUser>
    {
        public HoneypotDbContext(DbContextOptions<HoneypotDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }

        public DbSet<Quote> Quotes { get; set; }

        public DbSet<Author> Authors { get; set; }

        public DbSet<Bookshelf> Bookshelves { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<UsersQuotes> UsersQuotes { get; set; }

        public DbSet<BooksBookshelves> BooksBookshelves { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<BooksBookshelves>()
                .HasKey(x => new { x.BookId, x.BookshelfId });

            builder.Entity<UsersQuotes>()
                .HasKey(x => new { x.QuoteId, x.UserId });

            builder.Entity<Quote>()
                .HasOne(x => x.Author)
                .WithMany(y => y.Quotes)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Quote>()
                .HasOne(x => x.Book)
                .WithMany(y => y.Quotes)
                .OnDelete(DeleteBehavior.Restrict);

            //builder.Entity<Book>()
            //    .HasOne(x => x.Author)
            //    .WithMany(y => y.Books)
            //    .OnDelete(DeleteBehavior.Cascade);

            //builder.Entity<Review>()
            //    .HasOne(x => x.Owner)
            //    .WithMany(y => y.Reviews)
            //    .OnDelete(DeleteBehavior.SetNull);

            //builder.Entity<Review>()
            //    .HasOne(x => x.Book)
            //    .WithMany(y => y.Reviews)
            //    .OnDelete(DeleteBehavior.Cascade);

            //builder.Entity<Bookshelf>()
            //    .HasOne(x => x.Owner)
            //    .WithMany(y => y.Bookshelves)
            //    .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(builder);
        }
    }

    public class EventuresContextDbFactory : IDesignTimeDbContextFactory<HoneypotDbContext>
    {
        HoneypotDbContext IDesignTimeDbContextFactory<HoneypotDbContext>.CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<HoneypotDbContext>();
            optionsBuilder.UseSqlServer<HoneypotDbContext>(@"Server=DESKTOP-6P48I7L\SQLEXPRESS;Database=Honeypot;Trusted_Connection=True;MultipleActiveResultSets=true");

            return new HoneypotDbContext(optionsBuilder.Options);
        }
    }
}
