using System.Net;
using System.Net.Http.Json;
using Events.Application.Dtos;
using FluentAssertions;
using Xunit;

namespace Events.Api.E2ETests.Tests;

/// <summary>
/// Tests for read-only event query endpoints (recent, search, get by id).
/// </summary>
[Collection("EventsApi")]
public class EventQueryTests(EventsApiFactory factory) : IClassFixture<EventsApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    #region GET /api/event/recent

    [Fact]
    public async Task GetRecentEvents_ShouldReturnSeededEvents()
    {
        var response = await _client.GetAsync("/api/event/recent");
        response.EnsureSuccessStatusCode();

        var events = await response.Content.ReadFromJsonAsync<List<EventOverviewDto>>();
        events.Should().NotBeNull();
        events!.Should().NotBeEmpty();
        events.Select(e => e.Name).Should().Contain("Cozy Candlelit Dinner");
    }

    [Fact]
    public async Task GetRecentEvents_ShouldReturnOnlyActiveEvents()
    {
        var response = await _client.GetAsync("/api/event/recent");
        response.EnsureSuccessStatusCode();

        var events = await response.Content.ReadFromJsonAsync<List<EventOverviewDto>>();
        events.Should().NotBeNull();
        events!.Should().AllSatisfy(e => e.IsActive.Should().BeTrue());
    }

    [Fact]
    public async Task GetRecentEvents_ShouldIncludeEventDetails()
    {
        var response = await _client.GetAsync("/api/event/recent");
        response.EnsureSuccessStatusCode();

        var events = await response.Content.ReadFromJsonAsync<List<EventOverviewDto>>();
        events!.Should().AllSatisfy(e =>
        {
            e.EventId.Should().NotBeEmpty();
            e.Name.Should().NotBeNullOrEmpty();
            e.HostName.Should().NotBeNullOrEmpty();
        });
    }

    #endregion

    #region GET /api/event/search

    [Fact]
    public async Task SearchEvents_WithNoFilters_ShouldReturnEvents()
    {
        var response = await _client.GetAsync("/api/event/search");
        response.EnsureSuccessStatusCode();

        var events = await response.Content.ReadFromJsonAsync<List<EventOverviewDto>>();
        events.Should().NotBeNull();
        events!.Should().NotBeEmpty();
    }

    [Fact]
    public async Task SearchEvents_ByName_ShouldFilterResults()
    {
        var response = await _client.GetAsync("/api/event/search?locationOrEventName=Sushi");
        response.EnsureSuccessStatusCode();

        var events = await response.Content.ReadFromJsonAsync<List<EventOverviewDto>>();
        events.Should().NotBeNull();
        events!.Should().OnlyContain(e => e.Name.Contains("Sushi", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task SearchEvents_ByAgeRange_ShouldFilterResults()
    {
        var response = await _client.GetAsync("/api/event/search?minAge=21&maxAge=35");
        response.EnsureSuccessStatusCode();

        var events = await response.Content.ReadFromJsonAsync<List<EventOverviewDto>>();
        events.Should().NotBeNull();
        events!.Should().AllSatisfy(e =>
        {
            e.MinAllowedAge.Should().BeLessThanOrEqualTo(35);
            e.MaxAllowedAge.Should().BeGreaterThanOrEqualTo(21);
        });
    }

    [Fact]
    public async Task SearchEvents_ByPublic_ShouldFilterResults()
    {
        var response = await _client.GetAsync("/api/event/search?isPublic=true");
        response.EnsureSuccessStatusCode();

        var events = await response.Content.ReadFromJsonAsync<List<EventOverviewDto>>();
        events.Should().NotBeNull();
        events!.Should().AllSatisfy(e => e.IsPublic.Should().BeTrue());
    }

    [Fact]
    public async Task SearchEvents_WithNonMatchingFilter_ShouldReturnEmptyList()
    {
        var response = await _client.GetAsync("/api/event/search?locationOrEventName=NonExistentEventXYZ123");
        response.EnsureSuccessStatusCode();

        var events = await response.Content.ReadFromJsonAsync<List<EventOverviewDto>>();
        events.Should().NotBeNull();
        events!.Should().BeEmpty();
    }

    [Fact]
    public async Task SearchEvents_WithCombinedFilters_ShouldWork()
    {
        var response = await _client.GetAsync("/api/event/search?isPublic=true&minAge=18");
        response.EnsureSuccessStatusCode();

        var events = await response.Content.ReadFromJsonAsync<List<EventOverviewDto>>();
        events.Should().NotBeNull();
        events!.Should().AllSatisfy(e =>
        {
            e.IsPublic.Should().BeTrue();
            e.MinAllowedAge.Should().BeGreaterThanOrEqualTo(18);
        });
    }

    #endregion

    #region GET /api/event/{id}

    [Fact]
    public async Task GetEventById_WithNonExistentId_ShouldReturnNotFound()
    {
        var nonExistentId = Guid.NewGuid();
        var response = await _client.GetAsync($"/api/event/{nonExistentId}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    #endregion
}
