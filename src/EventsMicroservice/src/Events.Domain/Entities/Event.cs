using Events.Domain.Enums;

namespace Events.Domain.Entities;

public class Event
{
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string FoodName { get; set; } = default!;
    public EventStatus EventStatus { get; set; }
    public int MaxAllowedParticipants { get; set; }
    public int MinAllowedAge { get; set; }
    public int MaxAllowedAge { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime ReservationEndDate { get; set; }
    public string ImageThumbnail { get; set; } = default!;
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
}