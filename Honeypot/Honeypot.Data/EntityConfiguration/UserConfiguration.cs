using Honeypot.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Honeypot.Data.EntityConfiguration
{
    class UserConfiguration : IEntityTypeConfiguration<HoneypotUser>
    {
        public void Configure(EntityTypeBuilder<HoneypotUser> builder)
        {
            builder
                .HasMany(x => x.CustomBookshelves)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(x => x.LikedQuotes)
                .WithOne(x => x.User)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}