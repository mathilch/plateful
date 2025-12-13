using Azure.Messaging.ServiceBus;
using Events.Application.Contracts.Services;
using Events.Application.Dtos.Requests;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace Events.Application.Services;

public class NotificationServiceBusClient : INotificationsServiceBusClient
{
    private readonly ServiceBusClient _serviceBusClient;
    private readonly ServiceBusSender _sender;
    private readonly string _queueName;

    public NotificationServiceBusClient(
        IConfiguration configuration)
    {

        var connectionString = configuration["AzureServiceBus:ConnectionString"]
            ?? throw new InvalidOperationException("Azure Service Bus connection string is not configured.");

        _queueName = configuration["AzureServiceBus:QueueName"]
            ?? throw new InvalidOperationException("Azure Service Bus queue name is not configured.");

        _serviceBusClient = new ServiceBusClient(connectionString);
        _sender = _serviceBusClient.CreateSender(_queueName);
    }

    public async Task SendEmailNotification(NotificationRequestDto notificationRequestDto)
    {
        var messageBody = JsonSerializer.Serialize(notificationRequestDto);

        var message = new ServiceBusMessage(messageBody)
        {
            ContentType = "application/json",
            Subject = notificationRequestDto.Subject,
            MessageId = Guid.NewGuid().ToString()
        };

        await _sender.SendMessageAsync(message);
    }
}
