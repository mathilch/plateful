namespace Events.Domain;

public class FoodEvent
{
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public FoodEventStatus EventStatus { get; set; }
    public DateTime ReservationEndDate { get; set; }
    public int MaxAllowedParticipants { get; set; }
    public int MinAgeLimit { get; set; }
    public int MaxAgeLimit { get; set; }
    public string FoodName { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
}