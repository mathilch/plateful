using Events.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Events.Infrastructure.EntityConfigurations;

public class EventCommentEntityConfiguration : IEntityTypeConfiguration<EventComment>
{
    public void Configure(EntityTypeBuilder<EventComment> builder)
    {
        builder.HasKey(ec => ec.Id);
        builder.Property(ec => ec.EventId).IsRequired();
        builder.Property(ec => ec.UserId).IsRequired();

        builder.Property(ec => ec.Comment).IsRequired().HasMaxLength(500);

        builder.Property(e => e.CreatedDate)
        .HasDefaultValueSql("CURRENT_TIMESTAMP")
        .ValueGeneratedOnAdd()
        .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);
    }
}
