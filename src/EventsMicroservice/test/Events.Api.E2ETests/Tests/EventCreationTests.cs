using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Events.Application.Dtos;
using FluentAssertions;
using Xunit;

namespace Events.Api.E2ETests.Tests;

/// <summary>
/// Tests for event creation endpoint (POST /api/event).
/// </summary>
[Collection("EventsApi")]
public class EventCreationTests(EventsApiFactory factory) : IClassFixture<EventsApiFactory>, IAsyncDisposable
{
    private readonly HttpClient _client = factory.CreateClient();
    
    // Stores last response for logging in teardown
    private HttpResponseMessage? _response;
    private string? _responseBody;

    #region Teardown / Debug Logging

    public async ValueTask DisposeAsync()
    {
        if (_response is not null)
        {
            await LogResponse();
        }
    }

    private async Task LogResponse()
    {
        // Read body if not already read
        _responseBody ??= await _response!.Content.ReadAsStringAsync();
        
        // Try to pretty-print JSON
        string formattedBody;
        try
        {
            var json = JsonSerializer.Deserialize<JsonElement>(_responseBody);
            formattedBody = JsonSerializer.Serialize(json, new JsonSerializerOptions { WriteIndented = true });
        }
        catch
        {
            formattedBody = _responseBody;
        }

        var testName = TestContext.Current.Test?.TestDisplayName ?? "Unknown";
        TestContext.Current.TestOutputHelper?.WriteLine("═══════════════════════════════════════════════════════════");
        TestContext.Current.TestOutputHelper?.WriteLine($"Test: {testName}");
        TestContext.Current.TestOutputHelper?.WriteLine($"Status: {(int)_response!.StatusCode} {_response.StatusCode}");
        TestContext.Current.TestOutputHelper?.WriteLine($"Response Body:\n{formattedBody}");
        TestContext.Current.TestOutputHelper?.WriteLine("═══════════════════════════════════════════════════════════");
    }

    private async Task AssertBadRequest(string expectedMessage)
    {
        _response!.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        _responseBody = await _response.Content.ReadAsStringAsync();
        _responseBody.Should().Contain(expectedMessage);
    }

    #endregion

    #region Happy Path Tests

    [Fact]
    public async Task CreateEvent_WithValidData_ShouldReturnCreated()
    {
        var request = TestData.NewCreateEventRequest("New Test Event");
        _response = await _client.PostAsJsonAsync("/api/event", request);

        _response.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await _response.Content.ReadFromJsonAsync<EventDto>();

        TestData.AssertEventMatchesRequest(created!, request);
    }

    [Fact]
    public async Task CreateEvent_ThenGetById_ShouldRoundTrip()
    {
        var request = TestData.NewCreateEventRequest();
        _response = await _client.PostAsJsonAsync("/api/event", request);

        _response.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await _response.Content.ReadFromJsonAsync<EventDto>();
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
        _response = await _client.PostAsJsonAsync("/api/event", request);

        _response.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await _response.Content.ReadFromJsonAsync<EventDto>();

        TestData.AssertEventMatchesRequest(created!, request);
    }

    [Fact]
    public async Task CreateEvent_WithUnicodeCharacters_ShouldSucceed()
    {
        var request = TestData.NewCreateEventRequest("イベント 🍣 Événement невловимі свинію ямаленький гвинтикпес патрон");
        _response = await _client.PostAsJsonAsync("/api/event", request);

        _response.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await _response.Content.ReadFromJsonAsync<EventDto>();

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
        _response = await _client.PostAsJsonAsync("/api/event", request);

        await AssertBadRequest("Name"); // Either "Name is required" or "The Name field is required."
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public async Task CreateEvent_WithTooShortName_ShouldReturnBadRequest(int length)
    {
        var shortName = new string('A', length);
        var request = TestData.NewCreateEventRequestWith(name: shortName);
        _response = await _client.PostAsJsonAsync("/api/event", request);

        await AssertBadRequest("Name must be at least 3 characters");
    }

    [Theory]
    [InlineData(3)]   // minimum valid
    [InlineData(50)]  // normal
    [InlineData(150)] // maximum valid (db constraint)
    public async Task CreateEvent_WithValidNameLength_ShouldSucceed(int length)
    {
        var validName = new string('A', length);
        var request = TestData.NewCreateEventRequestWith(name: validName);
        _response = await _client.PostAsJsonAsync("/api/event", request);

        _response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Theory]
    [InlineData(151)]
    [InlineData(200)]
    [InlineData(500)]
    public async Task CreateEvent_WithTooLongName_ShouldReturnBadRequest(int length)
    {
        var longName = new string('A', length);
        var request = TestData.NewCreateEventRequestWith(name: longName);
        _response = await _client.PostAsJsonAsync("/api/event", request);

        await AssertBadRequest("Name cannot exceed 150 characters");
    }

    #endregion

    #region Description Validation Tests

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task CreateEvent_WithEmptyDescription_ShouldReturnBadRequest(string? invalidDescription)
    {
        var request = TestData.NewCreateEventRequestWith(description: invalidDescription);
        _response = await _client.PostAsJsonAsync("/api/event", request);

        await AssertBadRequest("Description"); // Either "Description is required" or "The Description field is required."
    }

    [Theory]
    [InlineData(151)]
    [InlineData(300)]
    public async Task CreateEvent_WithTooLongDescription_ShouldReturnBadRequest(int length)
    {
        var longDescription = new string('D', length);
        var request = TestData.NewCreateEventRequestWith(description: longDescription);
        _response = await _client.PostAsJsonAsync("/api/event", request);

        await AssertBadRequest("Description cannot exceed 150 characters");
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
        _response = await _client.PostAsJsonAsync("/api/event", request);

        await AssertBadRequest("MaxAllowedParticipants must be at least 1");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(100)]
    public async Task CreateEvent_WithValidParticipantCount_ShouldSucceed(int validCount)
    {
        var request = TestData.NewCreateEventRequestWith(maxAllowedParticipants: validCount);
        _response = await _client.PostAsJsonAsync("/api/event", request);

        _response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    #endregion

    #region PricePerSeat Validation Tests

    [Theory]
    [InlineData(-1)]
    [InlineData(-100.50)]
    public async Task CreateEvent_WithNegativePrice_ShouldReturnBadRequest(double invalidPrice)
    {
        var request = TestData.NewCreateEventRequestWith(pricePerSeat: invalidPrice);
        _response = await _client.PostAsJsonAsync("/api/event", request);

        await AssertBadRequest("PricePerSeat cannot be negative");
    }

    [Theory]
    [InlineData(0)]    // free event
    [InlineData(10.50)]
    [InlineData(100)]
    public async Task CreateEvent_WithValidPrice_ShouldSucceed(double validPrice)
    {
        var request = TestData.NewCreateEventRequestWith(pricePerSeat: validPrice);
        _response = await _client.PostAsJsonAsync("/api/event", request);

        _response.StatusCode.Should().Be(HttpStatusCode.Created);
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
        _response = await _client.PostAsJsonAsync("/api/event", request);

        await AssertBadRequest("cannot be negative");
    }

    [Theory]
    [InlineData(50, 18)]  // min > max
    [InlineData(30, 25)]
    public async Task CreateEvent_WithMinAgeGreaterThanMaxAge_ShouldReturnBadRequest(int minAge, int maxAge)
    {
        var request = TestData.NewCreateEventRequestWith(minAllowedAge: minAge, maxAllowedAge: maxAge);
        _response = await _client.PostAsJsonAsync("/api/event", request);

        await AssertBadRequest("MinAllowedAge cannot be greater than MaxAllowedAge");
    }

    [Theory]
    [InlineData(0, 100)]
    [InlineData(18, 65)]
    [InlineData(21, 21)] // same age is valid
    public async Task CreateEvent_WithValidAgeRange_ShouldSucceed(int minAge, int maxAge)
    {
        var request = TestData.NewCreateEventRequestWith(minAllowedAge: minAge, maxAllowedAge: maxAge);
        _response = await _client.PostAsJsonAsync("/api/event", request);

        _response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    #endregion

    #region Date Validation Tests

    [Fact]
    public async Task CreateEvent_WithStartDateInPast_ShouldReturnBadRequest()
    {
        var pastDate = DateTime.UtcNow.AddDays(-1);
        var request = TestData.NewCreateEventRequestWith(startDate: pastDate);
        _response = await _client.PostAsJsonAsync("/api/event", request);

        await AssertBadRequest("StartDate must be in the future");
    }

    [Fact]
    public async Task CreateEvent_WithReservationEndDateAfterStartDate_ShouldReturnBadRequest()
    {
        var startDate = DateTime.UtcNow.AddDays(7);
        var reservationEndDate = startDate.AddDays(1); // after start
        var request = TestData.NewCreateEventRequestWith(
            startDate: startDate,
            reservationEndDate: reservationEndDate);
        _response = await _client.PostAsJsonAsync("/api/event", request);

        await AssertBadRequest("ReservationEndDate must be before or on the StartDate");
    }

    [Fact]
    public async Task CreateEvent_WithEndDateBeforeStartDate_ShouldReturnBadRequest()
    {
        var startDate = DateTime.UtcNow.AddDays(7);
        var endDate = startDate.AddHours(-1); // before start
        var request = TestData.NewCreateEventRequestWith(
            startDate: startDate,
            endDate: endDate);
        _response = await _client.PostAsJsonAsync("/api/event", request);

        await AssertBadRequest("EndDate must be after StartDate");
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
        _response = await _client.PostAsJsonAsync("/api/event", request);

        _response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    #endregion

    #region Address Validation Tests

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task CreateEvent_WithEmptyStreetAddress_ShouldReturnBadRequest(string? invalidAddress)
    {
        var request = TestData.NewCreateEventRequestWith(streetAddress: invalidAddress);
        _response = await _client.PostAsJsonAsync("/api/event", request);

        await AssertBadRequest("StreetAddress"); // Either "StreetAddress is required" or "The StreetAddress field is required."
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task CreateEvent_WithEmptyCity_ShouldReturnBadRequest(string? invalidCity)
    {
        var request = TestData.NewCreateEventRequestWith(city: invalidCity);
        _response = await _client.PostAsJsonAsync("/api/event", request);

        await AssertBadRequest("City"); // Either "City is required" or "The City field is required."
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task CreateEvent_WithEmptyPostalCode_ShouldReturnBadRequest(string? invalidPostalCode)
    {
        var request = TestData.NewCreateEventRequestWith(postalCode: invalidPostalCode);
        _response = await _client.PostAsJsonAsync("/api/event", request);

        await AssertBadRequest("PostalCode"); // Either "PostalCode is required" or "The PostalCode field is required."
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task CreateEvent_WithEmptyRegion_ShouldReturnBadRequest(string? invalidRegion)
    {
        var request = TestData.NewCreateEventRequestWith(region: invalidRegion);
        _response = await _client.PostAsJsonAsync("/api/event", request);

        await AssertBadRequest("Region"); // Either "Region is required" or "The Region field is required."
    }

    [Fact]
    public async Task CreateEvent_WithTooLongStreetAddress_ShouldReturnBadRequest()
    {
        var longAddress = new string('A', 257); // max is 256
        var request = TestData.NewCreateEventRequestWith(streetAddress: longAddress);
        _response = await _client.PostAsJsonAsync("/api/event", request);

        await AssertBadRequest("StreetAddress cannot exceed 256 characters");
    }

    [Fact]
    public async Task CreateEvent_WithTooLongCity_ShouldReturnBadRequest()
    {
        var longCity = new string('C', 129); // max is 128
        var request = TestData.NewCreateEventRequestWith(city: longCity);
        _response = await _client.PostAsJsonAsync("/api/event", request);

        await AssertBadRequest("City cannot exceed 128 characters");
    }

    [Fact]
    public async Task CreateEvent_WithTooLongPostalCode_ShouldReturnBadRequest()
    {
        var longPostalCode = new string('1', 33); // max is 32
        var request = TestData.NewCreateEventRequestWith(postalCode: longPostalCode);
        _response = await _client.PostAsJsonAsync("/api/event", request);

        await AssertBadRequest("PostalCode cannot exceed 32 characters");
    }

    [Fact]
    public async Task CreateEvent_WithTooLongRegion_ShouldReturnBadRequest()
    {
        var longRegion = new string('R', 129); // max is 128
        var request = TestData.NewCreateEventRequestWith(region: longRegion);
        _response = await _client.PostAsJsonAsync("/api/event", request);

        await AssertBadRequest("Region cannot exceed 128 characters");
    }

    #endregion

    #region ImageThumbnail Validation Tests

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task CreateEvent_WithEmptyImageThumbnail_ShouldReturnBadRequest(string? invalidImage)
    {
        var request = TestData.NewCreateEventRequestWith(imageThumbnail: invalidImage);
        _response = await _client.PostAsJsonAsync("/api/event", request);

        await AssertBadRequest("ImageThumbnail"); // Either "ImageThumbnail is required" or "The ImageThumbnail field is required."
    }

    //TODO: change implementation and the test
    [Fact]
    public async Task CreateEvent_WithTooLongImageThumbnail_ShouldReturnBadRequest()
    {
        var longImage = new string('i', 151); // max is 150
        var request = TestData.NewCreateEventRequestWith(imageThumbnail: longImage);
        _response = await _client.PostAsJsonAsync("/api/event", request);

        await AssertBadRequest("ImageThumbnail cannot exceed 150 characters");
    }

    #endregion
}
