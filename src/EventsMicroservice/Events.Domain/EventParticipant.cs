namespace Events.Domain;

public class EventParticipant
{
    public int Id { get; set; }

    public int EventId { get; set; }
    public int UserId { get; set; }
    public DateTime CreateDate { get; set; }
    public int ParticipantStatus { get; set; }
    public int PaymentStatus { get; set; }
}