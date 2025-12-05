using System.Net.Http.Json;
using Events.Application.Dtos;
using FluentAssertions;
using Xunit;

namespace Events.Api.E2ETests.Tests;

/// <summary>
/// Tests for user-related event endpoints.
/// </summary>
[Collection("EventsApi")]
public class UserEventsTests(EventsApiFactory factory) : IClassFixture<EventsApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    #region GET /api/event/user/{userId}

    [Fact]
    public async Task GetEventsByUser_ShouldReturnUserEvents()
    {
        // Create an event first
        var payload = TestData.NewCreateEventRequest("User's Event");
        await _client.PostAsJsonAsync("/api/event", payload);

        // Get events for the current user
        var response = await _client.GetAsync($"/api/event/user/{TestUsers.DefaultUserId}");
        response.EnsureSuccessStatusCode();

        var events = await response.Content.ReadFromJsonAsync<List<EventOverviewDto>>();
        events.Should().NotBeNull();
        events!.Should().NotBeEmpty();
        events.Should().AllSatisfy(e => e.UserId.Should().Be(TestUsers.DefaultUserId));
    }

    [Fact]
    public async Task GetEventsByUser_WithNoEvents_ShouldReturnEmptyList()
    {
        var otherUserId = Guid.NewGuid();
        var response = await _client.GetAsync($"/api/event/user/{otherUserId}");
        response.EnsureSuccessStatusCode();

        var events = await response.Content.ReadFromJsonAsync<List<EventOverviewDto>>();
        events.Should().NotBeNull();
        events!.Should().BeEmpty();
    }

    [Fact]
    public async Task GetEventsByUser_ShouldIncludeAllUserEvents()
    {
        var eventNames = new[] { "User Event A", "User Event B", "User Event C" };

        // Create multiple events
        foreach (var name in eventNames)
        {
            var payload = TestData.NewCreateEventRequest(name);
            await _client.PostAsJsonAsync("/api/event", payload);
        }

        // Get events for the current user
        var response = await _client.GetAsync($"/api/event/user/{TestUsers.DefaultUserId}");
        var events = await response.Content.ReadFromJsonAsync<List<EventOverviewDto>>();

        foreach (var name in eventNames)
        {
            events!.Should().Contain(e => e.Name == name);
        }
    }

    #endregion

    #region GET /api/event/user-as-participant/{userId}

    [Fact]
    public async Task GetEventsAsParticipant_WhenNotParticipating_ShouldReturnEmptyList()
    {
        var otherUserId = Guid.NewGuid();
        var response = await _client.GetAsync($"/api/event/user-as-participant/{otherUserId}");
        response.EnsureSuccessStatusCode();

        var events = await response.Content.ReadFromJsonAsync<List<EventOverviewDto>>();
        events.Should().NotBeNull();
        events!.Should().BeEmpty();
    }

    #endregion

    #region GET /api/event/user-reviews-as-host/{userId}

    [Fact]
    public async Task GetUserReviewsAsHost_WithNoReviews_ShouldReturnEmptyList()
    {
        var response = await _client.GetAsync($"/api/event/user-reviews-as-host/{TestUsers.DefaultUserId}");
        response.EnsureSuccessStatusCode();

        var reviews = await response.Content.ReadFromJsonAsync<List<EventReviewDto>>();
        reviews.Should().NotBeNull();
    }

    [Fact]
    public async Task GetUserReviewsAsHost_WithNonExistentUser_ShouldReturnEmptyList()
    {
        var nonExistentUserId = Guid.NewGuid();
        var response = await _client.GetAsync($"/api/event/user-reviews-as-host/{nonExistentUserId}");
        response.EnsureSuccessStatusCode();

        var reviews = await response.Content.ReadFromJsonAsync<List<EventReviewDto>>();
        reviews.Should().NotBeNull();
        reviews!.Should().BeEmpty();
    }

    #endregion
}
