using System.Net;
using System.Net.Http.Json;
using Events.Application.Dtos;
using FluentAssertions;
using Xunit;

namespace Events.Api.E2ETests.Tests;

/// <summary>
/// Tests for event creation endpoint (POST /api/event).
/// </summary>
[Collection("EventsApi")]
public class EventCreationTests(EventsApiFactory factory) : IClassFixture<EventsApiFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    #region Happy Path Tests

    [Fact]
    public async Task CreateEvent_WithValidData_ShouldReturnCreated()
    {
        var request = TestData.NewCreateEventRequest("New Test Event");
        var response = await _client.PostAsJsonAsync("/api/event", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await response.Content.ReadFromJsonAsync<EventDto>();

        TestData.AssertEventMatchesRequest(created!, request);
    }

    [Fact]
    public async Task CreateEvent_ThenGetById_ShouldRoundTrip()
    {
        var request = TestData.NewCreateEventRequest();
        var createResponse = await _client.PostAsJsonAsync("/api/event", request);

        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await createResponse.Content.ReadFromJsonAsync<EventDto>();
        created.Should().NotBeNull();

        var getResponse = await _client.GetAsync($"/api/event/{created!.EventId}");
        getResponse.EnsureSuccessStatusCode();
        var fetched = await getResponse.Content.ReadFromJsonAsync<EventDto>();

        TestData.AssertEventMatchesRequest(fetched!, request);
    }

    [Fact]
    public async Task CreateEvent_WithSpecialCharacters_ShouldSucceed()
    {
        var request = TestData.NewCreateEventRequest("Event with @#$% & Special Chars!");
        var response = await _client.PostAsJsonAsync("/api/event", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await response.Content.ReadFromJsonAsync<EventDto>();

        TestData.AssertEventMatchesRequest(created!, request);
    }

    [Fact]
    public async Task CreateEvent_WithUnicodeCharacters_ShouldSucceed()
    {
        var request = TestData.NewCreateEventRequest("イベント 🍣 Événement невловимі свинію ямаленький гвинтикпес патрон");
        var response = await _client.PostAsJsonAsync("/api/event", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await response.Content.ReadFromJsonAsync<EventDto>();

        TestData.AssertEventMatchesRequest(created!, request);
    }

    #endregion

    #region Name Validation Tests

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")] // whitespace only
    public async Task CreateEvent_WithEmptyOrNullName_ShouldReturnBadRequest(string? invalidName)
    {
        var request = TestData.NewCreateEventRequestWith(name: invalidName);
        var response = await _client.PostAsJsonAsync("/api/event", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task CreateEvent_WithTooShortName_ShouldReturnBadRequest(int length)
    {
        var shortName = new string('A', length);
        var request = TestData.NewCreateEventRequestWith(name: shortName);
        var response = await _client.PostAsJsonAsync("/api/event", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData(3)]   // minimum valid
    [InlineData(50)]  // normal
    [InlineData(150)] // maximum valid (db constraint)
    public async Task CreateEvent_WithValidNameLength_ShouldSucceed(int length)
    {
        var validName = new string('A', length);
        var request = TestData.NewCreateEventRequestWith(name: validName);
        var response = await _client.PostAsJsonAsync("/api/event", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Theory]
    [InlineData(151)]
    [InlineData(200)]
    [InlineData(500)]
    public async Task CreateEvent_WithTooLongName_ShouldReturnBadRequest(int length)
    {
        var longName = new string('A', length);
        var request = TestData.NewCreateEventRequestWith(name: longName);
        var response = await _client.PostAsJsonAsync("/api/event", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion

    #region Description Validation Tests

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task CreateEvent_WithEmptyDescription_ShouldReturnBadRequest(string? invalidDescription)
    {
        var request = TestData.NewCreateEventRequestWith(description: invalidDescription);
        var response = await _client.PostAsJsonAsync("/api/event", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData(151)]
    [InlineData(300)]
    public async Task CreateEvent_WithTooLongDescription_ShouldReturnBadRequest(int length)
    {
        var longDescription = new string('D', length);
        var request = TestData.NewCreateEventRequestWith(description: longDescription);
        var response = await _client.PostAsJsonAsync("/api/event", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion

    #region MaxAllowedParticipants Validation Tests

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public async Task CreateEvent_WithInvalidParticipantCount_ShouldReturnBadRequest(int invalidCount)
    {
        var request = TestData.NewCreateEventRequestWith(maxAllowedParticipants: invalidCount);
        var response = await _client.PostAsJsonAsync("/api/event", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(100)]
    public async Task CreateEvent_WithValidParticipantCount_ShouldSucceed(int validCount)
    {
        var request = TestData.NewCreateEventRequestWith(maxAllowedParticipants: validCount);
        var response = await _client.PostAsJsonAsync("/api/event", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    #endregion

    #region PricePerSeat Validation Tests

    [Theory]
    [InlineData(-1)]
    [InlineData(-100.50)]
    public async Task CreateEvent_WithNegativePrice_ShouldReturnBadRequest(double invalidPrice)
    {
        var request = TestData.NewCreateEventRequestWith(pricePerSeat: invalidPrice);
        var response = await _client.PostAsJsonAsync("/api/event", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData(0)]    // free event
    [InlineData(10.50)]
    [InlineData(100)]
    public async Task CreateEvent_WithValidPrice_ShouldSucceed(double validPrice)
    {
        var request = TestData.NewCreateEventRequestWith(pricePerSeat: validPrice);
        var response = await _client.PostAsJsonAsync("/api/event", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    #endregion

    #region Age Range Validation Tests

    [Theory]
    [InlineData(-1, 50)]
    [InlineData(18, -1)]
    [InlineData(-5, -10)]
    public async Task CreateEvent_WithNegativeAge_ShouldReturnBadRequest(int minAge, int maxAge)
    {
        var request = TestData.NewCreateEventRequestWith(minAllowedAge: minAge, maxAllowedAge: maxAge);
        var response = await _client.PostAsJsonAsync("/api/event", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData(50, 18)]  // min > max
    [InlineData(30, 25)]
    public async Task CreateEvent_WithMinAgeGreaterThanMaxAge_ShouldReturnBadRequest(int minAge, int maxAge)
    {
        var request = TestData.NewCreateEventRequestWith(minAllowedAge: minAge, maxAllowedAge: maxAge);
        var response = await _client.PostAsJsonAsync("/api/event", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData(0, 100)]
    [InlineData(18, 65)]
    [InlineData(21, 21)] // same age is valid
    public async Task CreateEvent_WithValidAgeRange_ShouldSucceed(int minAge, int maxAge)
    {
        var request = TestData.NewCreateEventRequestWith(minAllowedAge: minAge, maxAllowedAge: maxAge);
        var response = await _client.PostAsJsonAsync("/api/event", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    #endregion

    #region Date Validation Tests

    [Fact]
    public async Task CreateEvent_WithStartDateInPast_ShouldReturnBadRequest()
    {
        var pastDate = DateTime.UtcNow.AddDays(-1);
        var request = TestData.NewCreateEventRequestWith(startDate: pastDate);
        var response = await _client.PostAsJsonAsync("/api/event", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateEvent_WithReservationEndDateAfterStartDate_ShouldReturnBadRequest()
    {
        var startDate = DateTime.UtcNow.AddDays(7);
        var reservationEndDate = startDate.AddDays(1); // after start
        var request = TestData.NewCreateEventRequestWith(
            startDate: startDate,
            reservationEndDate: reservationEndDate);
        var response = await _client.PostAsJsonAsync("/api/event", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateEvent_WithEndDateBeforeStartDate_ShouldReturnBadRequest()
    {
        var startDate = DateTime.UtcNow.AddDays(7);
        var endDate = startDate.AddHours(-1); // before start
        var request = TestData.NewCreateEventRequestWith(
            startDate: startDate,
            endDate: endDate);
        var response = await _client.PostAsJsonAsync("/api/event", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateEvent_WithValidDates_ShouldSucceed()
    {
        var startDate = DateTime.UtcNow.AddDays(7);
        var endDate = startDate.AddHours(3);
        var reservationEndDate = startDate.AddDays(-1);
        var request = TestData.NewCreateEventRequestWith(
            startDate: startDate,
            endDate: endDate,
            reservationEndDate: reservationEndDate);
        var response = await _client.PostAsJsonAsync("/api/event", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    #endregion

    #region Address Validation Tests

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task CreateEvent_WithEmptyStreetAddress_ShouldReturnBadRequest(string? invalidAddress)
    {
        var request = TestData.NewCreateEventRequestWith(streetAddress: invalidAddress);
        var response = await _client.PostAsJsonAsync("/api/event", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task CreateEvent_WithEmptyCity_ShouldReturnBadRequest(string? invalidCity)
    {
        var request = TestData.NewCreateEventRequestWith(city: invalidCity);
        var response = await _client.PostAsJsonAsync("/api/event", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task CreateEvent_WithEmptyPostalCode_ShouldReturnBadRequest(string? invalidPostalCode)
    {
        var request = TestData.NewCreateEventRequestWith(postalCode: invalidPostalCode);
        var response = await _client.PostAsJsonAsync("/api/event", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task CreateEvent_WithEmptyRegion_ShouldReturnBadRequest(string? invalidRegion)
    {
        var request = TestData.NewCreateEventRequestWith(region: invalidRegion);
        var response = await _client.PostAsJsonAsync("/api/event", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateEvent_WithTooLongStreetAddress_ShouldReturnBadRequest()
    {
        var longAddress = new string('A', 257); // max is 256
        var request = TestData.NewCreateEventRequestWith(streetAddress: longAddress);
        var response = await _client.PostAsJsonAsync("/api/event", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateEvent_WithTooLongCity_ShouldReturnBadRequest()
    {
        var longCity = new string('C', 129); // max is 128
        var request = TestData.NewCreateEventRequestWith(city: longCity);
        var response = await _client.PostAsJsonAsync("/api/event", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateEvent_WithTooLongPostalCode_ShouldReturnBadRequest()
    {
        var longPostalCode = new string('1', 33); // max is 32
        var request = TestData.NewCreateEventRequestWith(postalCode: longPostalCode);
        var response = await _client.PostAsJsonAsync("/api/event", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateEvent_WithTooLongRegion_ShouldReturnBadRequest()
    {
        var longRegion = new string('R', 129); // max is 128
        var request = TestData.NewCreateEventRequestWith(region: longRegion);
        var response = await _client.PostAsJsonAsync("/api/event", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion

    #region ImageThumbnail Validation Tests

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task CreateEvent_WithEmptyImageThumbnail_ShouldReturnBadRequest(string? invalidImage)
    {
        var request = TestData.NewCreateEventRequestWith(imageThumbnail: invalidImage);
        var response = await _client.PostAsJsonAsync("/api/event", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    //TODO: change implementation and the test
    [Fact]
    public async Task CreateEvent_WithTooLongImageThumbnail_ShouldReturnBadRequest()
    {
        var longImage = new string('i', 151); // max is 150
        var request = TestData.NewCreateEventRequestWith(imageThumbnail: longImage);
        var response = await _client.PostAsJsonAsync("/api/event", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    #endregion
}
