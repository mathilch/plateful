using Events.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Events.Infrastructure.EntityConfigurations;

public class EventFoodDetailsEntityConfiguration : IEntityTypeConfiguration<EventFoodDetails>
{
    public void Configure(EntityTypeBuilder<EventFoodDetails> builder)
    {
        builder.HasKey(ef => ef.Id);

        builder.Property(ef => ef.Ingredients).HasMaxLength(500);
        builder.Property(ef => ef.AdditionalFoodItems).HasMaxLength(500);

    }
}