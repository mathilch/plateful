namespace Events.Domain.Entities;

public class EventFoodDetails
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Event Event { get; set; } = default!;
    public string? Ingredients { get; set; }
    public string? AdditionalFoodItems { get; set; }
}