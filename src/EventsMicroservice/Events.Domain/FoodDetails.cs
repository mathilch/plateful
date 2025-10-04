namespace Events.Domain;

public class FoodDetails
{
    public int EventDetailsId { get; set; }
    public int EventId { get; set; }
    public string? Ingredients { get; set; }
    public string? AdditionalFoodItems { get; set; }

    // Navigation
    public Event? Event { get; set; }
}