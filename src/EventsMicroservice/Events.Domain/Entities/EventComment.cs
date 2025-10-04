namespace Events.Domain.Entities;

public class EventComment
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Event Event { get; set; } = default!;
    public Guid UserId { get; set; }
    public string Comment { get; set; } = default!;
    public DateTime CreatedDate { get; set; }
}
