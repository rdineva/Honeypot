using Honeypot.Models.MappingModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Honeypot.Data.EntityConfiguration
{
    public class UserQuoteConfiguration : IEntityTypeConfiguration<UserQuote>
    {
        public void Configure(EntityTypeBuilder<UserQuote> builder)
        {
            builder
                .HasKey(x => new { x.QuoteId, x.UserId });

            builder
                .HasOne(x => x.User)
                .WithMany(x => x.FavouriteQuotes)
                .HasForeignKey(x => x.UserId);

            builder
                .HasOne(x => x.Quote)
                .WithMany(x => x.LikedByUsers)
                .HasForeignKey(x => x.QuoteId);
        }
    }
}
