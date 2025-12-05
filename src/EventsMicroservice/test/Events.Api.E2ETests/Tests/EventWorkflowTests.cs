using System.Net;
using System.Net.Http.Json;
using Events.Application.Dtos;
using FluentAssertions;
using Xunit;

namespace Events.Api.E2ETests.Tests;

/// <summary>
/// Tests for multi-step event workflows and edge cases.
/// </summary>
[Collection("EventsApi")]
public class EventWorkflowTests(EventsApiFactory factory) : IClassFixture<EventsApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task CreateMultipleEvents_ShouldAllBeRetrievable()
    {
        var eventNames = new[] { "Breakfast Club", "Lunch Bunch", "Dinner Party" };
        var createdIds = new List<Guid>();

        // Create multiple events
        foreach (var name in eventNames)
        {
            var payload = TestData.NewCreateEventRequest(name);
            var response = await _client.PostAsJsonAsync("/api/event", payload);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var created = await response.Content.ReadFromJsonAsync<EventDto>();
            createdIds.Add(created!.EventId);
        }

        // Verify all events exist
        foreach (var id in createdIds)
        {
            var response = await _client.GetAsync($"/api/event/{id}");
            response.EnsureSuccessStatusCode();
        }

        // Get user's events - should contain all created events
        var userEventsResponse = await _client.GetAsync($"/api/event/user/{TestUsers.DefaultUserId}");
        var userEvents = await userEventsResponse.Content.ReadFromJsonAsync<List<EventOverviewDto>>();
        userEvents!.Select(e => e.EventId).Should().Contain(createdIds);
    }

    [Fact]
    public async Task GetRecentEvents_ShouldRespectPagination()
    {
        // Create several events to ensure we have more than one page
        for (int i = 0; i < 5; i++)
        {
            var payload = TestData.NewCreateEventRequest($"Pagination Test Event {i}");
            await _client.PostAsJsonAsync("/api/event", payload);
        }

        var response = await _client.GetAsync("/api/event/recent");
        var events = await response.Content.ReadFromJsonAsync<List<EventOverviewDto>>();

        // Should return at most 10 events (default page size)
        events!.Count.Should().BeLessThanOrEqualTo(10);
    }

    [Fact]
    public async Task GetEventById_AfterCreation_ShouldReturnCorrectData()
    {
        var payload = TestData.NewCreateEventRequest("Event For GetById Test");
        var createResponse = await _client.PostAsJsonAsync("/api/event", payload);
        var created = await createResponse.Content.ReadFromJsonAsync<EventDto>();

        var response = await _client.GetAsync($"/api/event/{created!.EventId}");
        response.EnsureSuccessStatusCode();

        var fetched = await response.Content.ReadFromJsonAsync<EventDto>();
        fetched.Should().NotBeNull();
        fetched!.EventId.Should().Be(created.EventId);
        fetched.Name.Should().Be(payload.Name);
    }
}
