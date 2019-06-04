using Honeypot.Models.MappingModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Honeypot.Data.EntityConfiguration
{
    public class BookBookshelfConfiguration : IEntityTypeConfiguration<BookBookshelf>
    {
        public void Configure(EntityTypeBuilder<BookBookshelf> builder)
        {
            builder
                .HasKey(x => new { x.BookId, x.BookshelfId });

            builder
                .HasOne(x => x.Book)
                .WithMany(x => x.InBookshelves)
                .HasForeignKey(x => x.BookId);

            builder
                .HasOne(x => x.Bookshelf)
                .WithMany(x => x.Books)
                .HasForeignKey(x => x.BookshelfId);
        }
    }
}
