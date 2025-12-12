using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Events.Application.Dtos;
using Events.Application.Dtos.Requests;
using FluentAssertions;
using Xunit;

namespace Events.Api.E2ETests.Tests;

/// <summary>
/// Expected validation result for declarative tests.
/// </summary>
public enum Expected
{
    Valid,
    TooShort,
    TooLong
}

/// <summary>
/// String field identifiers for declarative validation tests.
/// </summary>
public enum Field
{
    Name,
    Description,
    StreetAddress,
    City,
    PostalCode,
    Region,
    ImageThumbnail
}

/// <summary>
/// Tests for event creation endpoint (POST /api/event).
/// </summary>
[Collection("EventsApi")]
public class EventCreationTests(EventsApiFactory factory) : IClassFixture<EventsApiFactory>, IAsyncDisposable
{
    #region Setup / Helper methods

    private readonly HttpClient _client = factory.CreateClient();

    private HttpResponseMessage? _response;
    private string? _responseBody;

    public async ValueTask DisposeAsync()
    {
        if (_response is not null)
        {
            await LogResponse();
        }
    }

    private async Task LogResponse()
    {
        _responseBody ??= await _response!.Content.ReadAsStringAsync();

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

    private async Task AssertBadRequest(string shouldContain)
    {
        _response!.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        _responseBody = await _response.Content.ReadAsStringAsync();
        _responseBody.Should().Contain(shouldContain);
    }

    private async Task AssertBadRequest(string[] shouldContain)
    {
        _response!.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        _responseBody = await _response.Content.ReadAsStringAsync();
        _responseBody.Should().ContainAll(shouldContain);
    }

    #endregion

    #region Happy Path Tests

    [Fact]
    public async Task CreateEvent_WithValidData_ShouldReturnCreated()
    {
        var request = TestData.ValidRequest() with { Name = "New Test Event" };
        _response = await _client.PostAsJsonAsync("/api/event", request, TestContext.Current.CancellationToken);

        _response.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await _response.Content.ReadFromJsonAsync<EventOverviewDto>(TestContext.Current.CancellationToken);

        TestData.AssertEventMatchesRequest(created!, request);
    }

    [Fact]
    public async Task CreateEvent_ThenGetById_ShouldRoundTrip()
    {
        var request = TestData.ValidRequest();
        _response = await _client.PostAsJsonAsync("/api/event", request, TestContext.Current.CancellationToken);

        _response.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await _response.Content.ReadFromJsonAsync<EventOverviewDto>(TestContext.Current.CancellationToken);
        created.Should().NotBeNull();

        var getResponse =
            await _client.GetAsync($"/api/event/{created!.EventId}", TestContext.Current.CancellationToken);
        getResponse.EnsureSuccessStatusCode();
        var fetched = await getResponse.Content.ReadFromJsonAsync<EventOverviewDto>(TestContext.Current.CancellationToken);

        TestData.AssertEventMatchesRequest(fetched!, request);
    }

    [Fact]
    public async Task CreateEvent_WithSpecialCharacters_ShouldSucceed()
    {
        var request = TestData.ValidRequest() with { Name = "Event with @#$% & Special Chars!" };
        _response = await _client.PostAsJsonAsync("/api/event", request, TestContext.Current.CancellationToken);

        _response.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await _response.Content.ReadFromJsonAsync<EventOverviewDto>(TestContext.Current.CancellationToken);

        TestData.AssertEventMatchesRequest(created!, request);
    }

    [Fact]
    public async Task CreateEvent_WithUnicodeCharacters_ShouldSucceed()
    {
        var request = TestData.ValidRequest() with
        {
            Name = "イベント 🍣 Événement невловимі свинію ямаленький гвинтикпес патрон"
        };
        _response = await _client.PostAsJsonAsync("/api/event", request, TestContext.Current.CancellationToken);

        _response.StatusCode.Should().Be(HttpStatusCode.Created);
        var created = await _response.Content.ReadFromJsonAsync<EventOverviewDto>(TestContext.Current.CancellationToken);

        TestData.AssertEventMatchesRequest(created!, request);
    }

    #endregion

    #region Declarative String Length Validation Tests

    [Theory]
    // Name: min 3, max 150
    [InlineData(Field.Name, 1, Expected.TooShort)]
    [InlineData(Field.Name, 2, Expected.TooShort)]
    [InlineData(Field.Name, 3, Expected.Valid)]
    [InlineData(Field.Name, 150, Expected.Valid)]
    [InlineData(Field.Name, 151, Expected.TooLong)]
    // Description: max 150
    [InlineData(Field.Description, 1, Expected.Valid)]
    [InlineData(Field.Description, 150, Expected.Valid)]
    [InlineData(Field.Description, 151, Expected.TooLong)]
    // StreetAddress: max 256
    [InlineData(Field.StreetAddress, 1, Expected.Valid)]
    [InlineData(Field.StreetAddress, 256, Expected.Valid)]
    [InlineData(Field.StreetAddress, 257, Expected.TooLong)]
    // City: max 128
    [InlineData(Field.City, 1, Expected.Valid)]
    [InlineData(Field.City, 128, Expected.Valid)]
    [InlineData(Field.City, 129, Expected.TooLong)]
    // PostalCode: max 32
    [InlineData(Field.PostalCode, 1, Expected.Valid)]
    [InlineData(Field.PostalCode, 32, Expected.Valid)]
    [InlineData(Field.PostalCode, 33, Expected.TooLong)]
    // Region: max 128
    [InlineData(Field.Region, 1, Expected.Valid)]
    [InlineData(Field.Region, 128, Expected.Valid)]
    [InlineData(Field.Region, 129, Expected.TooLong)]
    // ImageThumbnail: max 150
    [InlineData(Field.ImageThumbnail, 1, Expected.Valid)]
    [InlineData(Field.ImageThumbnail, 150, Expected.Valid)]
    [InlineData(Field.ImageThumbnail, 151, Expected.TooLong)]
    public async Task CreateEvent_StringLengthValidation(Field field, int length, Expected expected)
    {
        var value = new string('X', length);
        var request = WithField(TestData.ValidRequest(), field, value);
        _response = await _client.PostAsJsonAsync("/api/event", request, TestContext.Current.CancellationToken);

        switch (expected)
        {
            case Expected.Valid:
                _response.StatusCode.Should().Be(HttpStatusCode.Created,
                    $"{field} with length {length} should be valid.");
                break;
            case Expected.TooShort:
                // FluentValidation: "The length of 'Name' must be at least 3 characters. You entered 2 characters."
                await AssertBadRequest([field.ToString(), "must be at least", $"You entered {length} characters"]);
                break;
            case Expected.TooLong:
                // FluentValidation: "The length of 'City' must be 128 characters or fewer. You entered 129 characters."
                await AssertBadRequest([field.ToString(), "characters or fewer", $"You entered {length} characters"]);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(expected), expected, null);
        }
    }

    [Theory]
    [InlineData(Field.Name)]
    [InlineData(Field.Description)]
    [InlineData(Field.StreetAddress)]
    [InlineData(Field.City)]
    [InlineData(Field.PostalCode)]
    [InlineData(Field.Region)]
    [InlineData(Field.ImageThumbnail)]
    public async Task CreateEvent_RequiredFieldValidation(Field field)
    {
        foreach (var requiredTestCaseVariant in new[] { null, "", "  " })
        {
            var request = WithField(TestData.ValidRequest(), field, requiredTestCaseVariant);
            _response = await _client.PostAsJsonAsync("/api/event", request, TestContext.Current.CancellationToken);

            await AssertBadRequest(field.ToString());
        }
    }

    private static CreateEventRequestDto WithField(CreateEventRequestDto request, Field field, string? value) =>
        field switch
        {
            Field.Name => request with { Name = value! },
            Field.Description => request with { Description = value! },
            Field.StreetAddress => request with { StreetAddress = value! },
            Field.City => request with { City = value! },
            Field.PostalCode => request with { PostalCode = value! },
            Field.Region => request with { Region = value! },
            Field.ImageThumbnail => request with { ImageThumbnail = value! },
            _ => throw new ArgumentOutOfRangeException(nameof(field))
        };

    #endregion

    #region Numeric Validation Tests

    [Theory]
    [InlineData(0, false)]
    [InlineData(-1, false)]
    [InlineData(-100, false)]
    [InlineData(1, true)]
    [InlineData(10, true)]
    [InlineData(100, true)]
    [InlineData(101, false)]
    public async Task CreateEvent_MaxParticipantsValidation(int count, bool shouldBeValid)
    {
        var request = TestData.ValidRequest() with { MaxAllowedParticipants = count };
        _response = await _client.PostAsJsonAsync("/api/event", request, TestContext.Current.CancellationToken);

        if (shouldBeValid)
            _response.StatusCode.Should().Be(HttpStatusCode.Created);
        else if (count > 100)
            await AssertBadRequest("must be less than or equal to"); 
        else
            await AssertBadRequest("greater than '0'"); // FluentValidation default: "'X' must be greater than '0'."
    }

    [Theory]
    [InlineData(-1, false)]
    [InlineData(-100.50, false)]
    [InlineData(0, true)]
    [InlineData(10.50, true)]
    [InlineData(100, true)]
    public async Task CreateEvent_PriceValidation(double price, bool shouldBeValid)
    {
        var request = TestData.ValidRequest() with { PricePerSeat = price };
        _response = await _client.PostAsJsonAsync("/api/event", request, TestContext.Current.CancellationToken);

        if (shouldBeValid)
            _response.StatusCode.Should().Be(HttpStatusCode.Created);
        else
            await AssertBadRequest("greater than or equal to '0'"); // FluentValidation default
    }

    #endregion

    #region Age Range Validation Tests

    [Theory]
    [InlineData(-1, 50)]
    [InlineData(18, -1)]
    [InlineData(-5, -10)]
    public async Task CreateEvent_WithNegativeAge_ShouldReturnBadRequest(int minAge, int maxAge)
    {
        var request = TestData.ValidRequest() with { MinAllowedAge = minAge, MaxAllowedAge = maxAge };
        _response = await _client.PostAsJsonAsync("/api/event", request, TestContext.Current.CancellationToken);

        await AssertBadRequest("greater than or equal to '0'"); // FluentValidation default
    }

    [Theory]
    [InlineData(50, 18)]
    [InlineData(30, 25)]
    public async Task CreateEvent_WithMinAgeGreaterThanMaxAge_ShouldReturnBadRequest(int minAge, int maxAge)
    {
        var request = TestData.ValidRequest() with { MinAllowedAge = minAge, MaxAllowedAge = maxAge };
        _response = await _client.PostAsJsonAsync("/api/event", request, TestContext.Current.CancellationToken);

        await AssertBadRequest("less than or equal to"); // FluentValidation default for cross-field
    }

    [Theory]
    [InlineData(0, 100)]
    [InlineData(18, 65)]
    [InlineData(21, 21)]
    [InlineData(0, 150)]
    public async Task CreateEvent_ValidAgeRange_ShouldSucceed(int minAge, int maxAge)
    {
        var request = TestData.ValidRequest() with { MinAllowedAge = minAge, MaxAllowedAge = maxAge };
        _response = await _client.PostAsJsonAsync("/api/event", request, TestContext.Current.CancellationToken);

        _response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    #endregion

    #region Date Validation Tests

    [Fact]
    public async Task CreateEvent_WithStartDateInPast_ShouldReturnBadRequest()
    {
        var pastDate = DateTime.UtcNow.AddDays(-1);
        var request = TestData.ValidRequest() with { StartDate = pastDate };
        _response = await _client.PostAsJsonAsync("/api/event", request, TestContext.Current.CancellationToken);

        await AssertBadRequest("Start Date"); // FluentValidation uses "Start Date" in message
    }

    [Fact]
    public async Task CreateEvent_WithReservationEndDateAfterStartDate_ShouldReturnBadRequest()
    {
        var startDate = DateTime.UtcNow.AddDays(7);
        var request = TestData.ValidRequest() with
        {
            StartDate = startDate,
            ReservationEndDate = startDate.AddDays(1)
        };
        _response = await _client.PostAsJsonAsync("/api/event", request, TestContext.Current.CancellationToken);

        await AssertBadRequest("Reservation End Date");
    }

    [Fact]
    public async Task CreateEvent_WithEndDateBeforeStartDate_ShouldReturnBadRequest()
    {
        var startDate = DateTime.UtcNow.AddDays(7);
        var request = TestData.ValidRequest() with
        {
            StartDate = startDate,
            EndDate = startDate.AddHours(-1)
        };
        _response = await _client.PostAsJsonAsync("/api/event", request, TestContext.Current.CancellationToken);

        await AssertBadRequest("End Date");
    }

    [Fact]
    public async Task CreateEvent_WithValidDates_ShouldSucceed()
    {
        var startDate = DateTime.UtcNow.AddDays(7);
        var request = TestData.ValidRequest() with
        {
            StartDate = startDate,
            EndDate = startDate.AddHours(3),
            ReservationEndDate = startDate.AddDays(-1)
        };
        _response = await _client.PostAsJsonAsync("/api/event", request, TestContext.Current.CancellationToken);

        _response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    #endregion
}