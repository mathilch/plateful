using Events.Application.Contracts.ExternalApis;
using Events.Application.Dtos;
using Stripe;

namespace Events.Api.E2ETests;

internal class FakeUserApiService : IUserApiService
{
    public Task<IEnumerable<UserDto>> GetUsersByIds(IEnumerable<Guid> userIds)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var users = userIds.Select(id =>
            new UserDto(
                id,
                id == TestUsers.DefaultUserId ? "Test User" : "Seed Host",
                "user@example.com",
                today.AddYears(-30),
                DateTime.UtcNow.AddYears(-1),
                true));

        return Task.FromResult(users);
    }
}

internal class FakePaymentService : IPaymentService
{
    public Task<PaymentIntent> CreatePaymentIntent(long amount, Dictionary<string, string> metadata) =>
        Task.FromResult(new PaymentIntent
        {
            Id = Guid.NewGuid().ToString(),
            Amount = amount,
            Metadata = metadata
        });
}

