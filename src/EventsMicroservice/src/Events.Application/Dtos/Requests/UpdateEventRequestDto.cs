namespace Events.Application.Dtos.Requests;

public class UpdateEventRequestDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string FoodName { get; set; }
    public int MaxAllowedParticipants { get; set; }
    public int MinAllowedAge { get; set; }
    public int MaxAllowedAge { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime ReservationEndDate { get; set; }
    public string ImageThumbnailUrl { get; set; }
    
}