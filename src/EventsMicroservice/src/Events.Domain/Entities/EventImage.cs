namespace Events.Domain.Entities;

public class EventImage
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Event Event { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string RelativeUrl { get; set; } = default!;
    public DateTime CreatedDate { get; set; }
}