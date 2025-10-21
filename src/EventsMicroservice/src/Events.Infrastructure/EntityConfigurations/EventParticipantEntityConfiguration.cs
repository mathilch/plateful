using Events.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Entities;

namespace Events.Infrastructure.EntityConfigurations;

public class EventParticipantEntityConfiguration : IEntityTypeConfiguration<EventParticipant>
{
    public void Configure(EntityTypeBuilder<EventParticipant> builder)
    {
        builder.ToTable("EventParticipants");
        
        builder.HasKey(ep => ep.EventId);
        
        builder.HasOne<Event>()
            .WithMany(e => e.EventParticipants)
            .HasForeignKey(ep => ep.EventId)
            .IsRequired();
        
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(ep => ep.UserId)
            .IsRequired();
        
        builder.HasIndex(ep => new { ep.EventId, ep.UserId }).IsUnique();
        
        builder.Property(ep => ep.CreatedDate).IsRequired();
        builder.Property(ep => ep.ParticipantStatus)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(ep => ep.PaymentStatus)
            .HasConversion<string>()
            .IsRequired();
    }
    
}