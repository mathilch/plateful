using System.Net;
using System.Net.Http.Json;
using Events.Application.Dtos;
using FluentAssertions;
using Xunit;

namespace Events.Api.E2ETests.Tests;

/// <summary>
/// Tests for event deletion endpoint (DELETE /api/event/{id}).
/// </summary>
[Collection("EventsApi")]
public class EventDeletionTests(EventsApiFactory factory) : IClassFixture<EventsApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task DeleteEvent_ByOwner_ShouldSucceed()
    {
        // Create an event
        var payload = TestData.ValidRequest() with { Name = "Event To Delete" };
        var createResponse = await _client.PostAsJsonAsync("/api/event", payload, cancellationToken: TestContext.Current.CancellationToken);
        var created = await createResponse.Content.ReadFromJsonAsync<EventDto>(cancellationToken: TestContext.Current.CancellationToken);

        // Delete it
        var deleteResponse = await _client.DeleteAsync($"/api/event/{created!.EventId}", TestContext.Current.CancellationToken);
        deleteResponse.EnsureSuccessStatusCode();

        var deleted = await deleteResponse.Content.ReadFromJsonAsync<EventDto>(cancellationToken: TestContext.Current.CancellationToken);
        deleted.Should().NotBeNull();
        deleted!.EventId.Should().Be(created.EventId);
    }
    
    [Fact]
    public async Task DeleteEvent_WithNonExistentId_ShouldReturnNotFound()
    {
        var nonExistentId = Guid.NewGuid();
        var response = await _client.DeleteAsync($"/api/event/{nonExistentId}", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteEvent_ThenGetById_ShouldReturnNotFound()
    {
        // Create an event
        var payload = TestData.ValidRequest() with { Name = "Event To Delete And Verify" };
        var createResponse = await _client.PostAsJsonAsync("/api/event", payload, cancellationToken: TestContext.Current.CancellationToken);
        var created = await createResponse.Content.ReadFromJsonAsync<EventDto>(cancellationToken: TestContext.Current.CancellationToken);

        // Delete it
        await _client.DeleteAsync($"/api/event/{created!.EventId}", TestContext.Current.CancellationToken);

        // Try to get it again
        var getResponse = await _client.GetAsync($"/api/event/{created.EventId}", TestContext.Current.CancellationToken);
        getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateAndDelete_ShouldNotAffectOtherEvents()
    {
        // Create two events
        var payload1 = TestData.ValidRequest() with { Name = "Event To Keep" };
        var response1 = await _client.PostAsJsonAsync("/api/event", payload1, cancellationToken: TestContext.Current.CancellationToken);
        var created1 = await response1.Content.ReadFromJsonAsync<EventDto>(cancellationToken: TestContext.Current.CancellationToken);

        var payload2 = TestData.ValidRequest() with { Name = "Event To Delete" };
        var response2 = await _client.PostAsJsonAsync("/api/event", payload2, cancellationToken: TestContext.Current.CancellationToken);
        var created2 = await response2.Content.ReadFromJsonAsync<EventDto>(cancellationToken: TestContext.Current.CancellationToken);

        // Delete the second event
        await _client.DeleteAsync($"/api/event/{created2!.EventId}", TestContext.Current.CancellationToken);

        // First event should still exist
        var getResponse = await _client.GetAsync($"/api/event/{created1!.EventId}", TestContext.Current.CancellationToken);
        getResponse.EnsureSuccessStatusCode();
        var fetched = await getResponse.Content.ReadFromJsonAsync<EventDto>(cancellationToken: TestContext.Current.CancellationToken);
        fetched!.Name.Should().Be(payload1.Name);
    }
}
