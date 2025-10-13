using Microsoft.EntityFrameworkCore;
using Events.Domain.Entities;
using Events.Infrastructure.EntityConfigurations;

namespace Events.Infrastructure.Context;

public class EventsDbContext : DbContext
{
    public DbSet<Event> Events => Set<Event>();

    public EventsDbContext(DbContextOptions<EventsDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EventEntityConfiguration());

    }
}
