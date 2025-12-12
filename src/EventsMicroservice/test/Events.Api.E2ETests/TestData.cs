using Events.Application.Dtos;
using Events.Application.Dtos.Requests;
using Events.Domain.Entities;
using FluentAssertions;

namespace Events.Api.E2ETests;

internal static class TestUsers
{
    public static readonly Guid DefaultUserId = Guid.Parse("aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee");
}

internal static class TestData
{
    private static DateTime DefaultStartDate => DateTime.UtcNow.AddDays(7).Date.AddHours(18);

    /// <summary>
    /// Creates a valid CreateEventRequestDto with default values.
    /// Use with expression to override: TestData.ValidRequest() with { Name = "Custom" }
    /// </summary>
    public static CreateEventRequestDto ValidRequest() => new(
        Name: "Integration Feast",
        Description: "Dinner with integration tests",
        MaxAllowedParticipants: 8,
        PricePerSeat: 25,
        MinAllowedAge: 18,
        MaxAllowedAge: 80,
        StartDate: DefaultStartDate,
        EndDate: null,
        ReservationEndDate: DefaultStartDate.AddDays(-1),
        ImageThumbnail: "thumbnail.png",
        IsPublic: true,
        EventFoodDetails: new EventFoodDetails
        {
            Name = "Tasting Menu",
            Ingredients = "Pasta,Tomato,Cheese",
            DietaryStyles = new List<string> { "Vegetarian" },
            Allergens = new List<string> { "Gluten" },
            AdditionalFoodItems = "Sparkling water"
        },
        Images: Array.Empty<EventImage>(),
        StreetAddress: "123 Test Street",
        PostalCode: "12345",
        City: "Testville",
        Region: "TestState"
    );

    /// <summary>
    /// Asserts that an EventOverviewDto matches the expected values from a CreateEventRequestDto.
    /// </summary>
    public static void AssertEventMatchesRequest(EventOverviewDto actual, CreateEventRequestDto expected)
    {
        actual.Should().NotBeNull();
        actual.Name.Should().Be(expected.Name);
        actual.Description.Should().Be(expected.Description);
        actual.MaxAllowedParticipants.Should().Be(expected.MaxAllowedParticipants);
        actual.PricePerSeat.Should().Be(expected.PricePerSeat);
        actual.MinAllowedAge.Should().Be(expected.MinAllowedAge);
        actual.MaxAllowedAge.Should().Be(expected.MaxAllowedAge);
        actual.IsPublic.Should().Be(expected.IsPublic);
        actual.StartDate.Should().Be(expected.StartDate.Date.ToString("dd/MM/yyyy"));
        actual.StartTime.Should().Be(expected.StartDate.ToString("hh:mm"));
        actual.ReservationEndDate.Should().Be(expected.ReservationEndDate);
        actual.IsActive.Should().BeTrue();
        actual.UserId.Should().Be(TestUsers.DefaultUserId);

        // Address
        actual.EventAddress.Should().NotBeNull();
        actual.EventAddress.City.Should().Be(expected.City);
        actual.EventAddress.StreetAddress.Should().Be(expected.StreetAddress);
        actual.EventAddress.PostalCode.Should().Be(expected.PostalCode);
        actual.EventAddress.Region.Should().Be(expected.Region);
    }
}
