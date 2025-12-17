using Events.Application.Contracts.ExternalApis;
using Microsoft.Extensions.Options;
using Stripe;

namespace Events.Infrastructure.ExternalApis;

public class StripeService : IPaymentService
{
    private readonly StripeSettings _stripeSettings;

    public StripeService(IOptions<StripeSettings> stripeSettings)
    {
        _stripeSettings = stripeSettings.Value;
    }
    public async Task<PaymentIntent> CreatePaymentIntent(long amount, Dictionary<string, string> metadata)
    {
        StripeConfiguration.ApiKey = _stripeSettings.SecretKey;
        var options = new PaymentIntentCreateOptions
        {
            Amount = amount,
            Currency = "dkk",
            Metadata = metadata,
            AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
            {
                Enabled = true,
            },
        };

        var service = new PaymentIntentService();
        return await service.CreateAsync(options);
    }
}