using Events.Application.Contracts.ExternalApis;
using Stripe;

namespace Events.Infrastructure.ExternalApis;

public class StripeService : IPaymentService
{
    private readonly StripeSettings _stripeSettings;

    public StripeService(StripeSettings stripeSettings)
    {
        _stripeSettings = stripeSettings;
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