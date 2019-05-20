using Honeypot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Honeypot.Data.EntityConfiguration
{
    class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder
                .HasOne(x => x.Author)
                .WithMany(x => x.Books)
                .HasForeignKey(x => x.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(x => x.InBookshelves)
                .WithOne(x => x.Book)
                .HasForeignKey(x => x.BookshelfId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(x => x.Quotes)
                .WithOne(x => x.Book)
                .HasForeignKey(x => x.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(x => x.Ratings)
                .WithOne(x => x.Book)
                .HasForeignKey(x => x.Id)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
