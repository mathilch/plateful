using Events.Application.Dtos.Requests;

namespace Events.Application.Contracts.Services;

public interface INotificationsServiceBusClient
{
    Task SendEmailNotification(NotificationRequestDto notificationRequestDto);
}
