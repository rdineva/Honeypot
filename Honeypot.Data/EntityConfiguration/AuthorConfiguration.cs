using Honeypot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Honeypot.Data.EntityConfiguration
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder
                .HasKey(x => x.Id);

            /*builder
                .HasMany(x => x.Books)
                .WithOne(x => x.Author)
                .OnDelete(DeleteBehavior.Cascade);*/
        }
    }
}