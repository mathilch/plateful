using Azure;
using Azure.Communication.Email;
using Microsoft.Extensions.Options;
using NotificationFunction.Options;

namespace NotificationFunction.Services;

public class AcsEmailSender : IEmailSender
{
    private readonly EmailClient _client;
    private readonly AcsEmailOptions _options;

    public AcsEmailSender(IOptions<AcsEmailOptions> options)
    {
        _options = options.Value;
        _client = new EmailClient(_options.ConnectionString);
    }

    public async Task SendAsync(string to, string subject, string htmlBody, CancellationToken cancellationToken = default)
    {
        var content = new EmailContent(subject)
        {
            Html = htmlBody
        };

        var from = _options.FromAddress!;
        var toList = new EmailRecipients(new[] { new EmailAddress(to) });

        var message = new EmailMessage(from, toList, content);

        // Optional: Set the sender display name via headers
        if (!string.IsNullOrWhiteSpace(_options.FromDisplayName))
        {
            message.Headers["From"] = $"{_options.FromDisplayName} <{from}>";
        }

        // Send and optionally wait for delivery status
        var response = await _client.SendAsync(WaitUntil.Completed, message, cancellationToken);

        if (response.Value.Status != EmailSendStatus.Succeeded)
        {
            throw new System.InvalidOperationException(
                $"ACS send failed: Status={response.Value.Status}");
        }
    }
}
