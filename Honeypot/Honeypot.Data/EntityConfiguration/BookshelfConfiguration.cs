using System;
using Honeypot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Honeypot.Data.EntityConfiguration
{
    public class BookshelfConfiguration : IEntityTypeConfiguration<Bookshelf>
    {
        public void Configure(EntityTypeBuilder<Bookshelf> builder)
        {
           builder
               .HasOne(x => x.User)
               .WithMany(x => x.CustomBookshelves)
               .HasForeignKey(x => x.UserId);

            builder
                .HasMany(x => x.Books)
                .WithOne(x => x.Bookshelf)
                .HasForeignKey(x => x.BookshelfId);
        }
    }
}
