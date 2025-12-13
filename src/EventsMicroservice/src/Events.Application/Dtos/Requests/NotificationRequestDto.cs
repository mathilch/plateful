namespace Events.Application.Dtos.Requests;

public class NotificationRequestDto
{
    public string Subject { get; set; } = default!;
    public string ToAddress { get; set; } = default!;
    public string EmailContent { get; set; } = default!;
}
