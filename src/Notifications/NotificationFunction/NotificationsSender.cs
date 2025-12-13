using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using NotificationFunction.Dtos;
using NotificationFunction.Services;
using System.Text.Json;

namespace NotificationFunction;

public class NotificationsSender
{
    private readonly ILogger<NotificationsSender> _logger;
    private readonly IEmailSender _emailSender;

    public NotificationsSender(ILogger<NotificationsSender> logger, IEmailSender emailSender)
    {
        _logger = logger;
        _emailSender = emailSender;
    }

    [Function(nameof(NotificationsSender))]
    public async Task Run(
        [ServiceBusTrigger("notifications-queue", Connection = "service-bus-connection")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Message ID: {id}", message.MessageId);
        _logger.LogInformation("Message Body: {body}", message.Body);
        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        var body = JsonSerializer.Deserialize<NotificationRequestDto>(message.Body.ToString());
        if (body is not null)
        {
            await _emailSender.SendAsync(body.ToAddress, body.Subject, body.EmailContent);
        }
        await messageActions.CompleteMessageAsync(message);
    }
}