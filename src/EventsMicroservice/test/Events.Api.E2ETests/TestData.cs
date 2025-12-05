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

    public static CreateEventRequestDto NewCreateEventRequest(string name = "Integration Feast")
    {
        return new CreateEventRequestDto(
            Name: name,
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
    }

    /// <summary>
    /// Creates a CreateEventRequestDto with customizable fields for validation testing.
    /// </summary>
    public static CreateEventRequestDto NewCreateEventRequestWith(
        string? name = "Integration Feast",
        string? description = "Dinner with integration tests",
        int maxAllowedParticipants = 8,
        double pricePerSeat = 25,
        int minAllowedAge = 18,
        int maxAllowedAge = 80,
        DateTime? startDate = null,
        DateTime? endDate = null,
        DateTime? reservationEndDate = null,
        string? imageThumbnail = "thumbnail.png",
        bool isPublic = true,
        EventFoodDetails? eventFoodDetails = null,
        string? streetAddress = "123 Test Street",
        string? postalCode = "12345",
        string? city = "Testville",
        string? region = "TestState")
    {
        var start = startDate ?? DefaultStartDate;
        return new CreateEventRequestDto(
            Name: name!,
            Description: description!,
            MaxAllowedParticipants: maxAllowedParticipants,
            PricePerSeat: pricePerSeat,
            MinAllowedAge: minAllowedAge,
            MaxAllowedAge: maxAllowedAge,
            StartDate: start,
            EndDate: endDate,
            ReservationEndDate: reservationEndDate ?? start.AddDays(-1),
            ImageThumbnail: imageThumbnail!,
            IsPublic: isPublic,
            EventFoodDetails: eventFoodDetails ?? new EventFoodDetails
            {
                Name = "Tasting Menu",
                Ingredients = "Pasta,Tomato,Cheese",
                DietaryStyles = new List<string> { "Vegetarian" },
                Allergens = new List<string> { "Gluten" },
                AdditionalFoodItems = "Sparkling water"
            },
            Images: Array.Empty<EventImage>(),
            StreetAddress: streetAddress!,
            PostalCode: postalCode!,
            City: city!,
            Region: region!
        );
    }

    /// <summary>
    /// Asserts that an EventDto matches the expected values from a CreateEventRequestDto.
    /// </summary>
    /// <param name="actual">The actual EventDto returned from the API.</param>
    /// <param name="expected">The expected CreateEventRequestDto.</param>
    public static void AssertEventMatchesRequest(EventDto actual, CreateEventRequestDto expected)
    {
        actual.Should().NotBeNull();
        actual.Name.Should().Be(expected.Name);
        actual.Description.Should().Be(expected.Description);
        actual.MaxAllowedParticipants.Should().Be(expected.MaxAllowedParticipants);
        actual.PricePerSeat.Should().Be(expected.PricePerSeat);
        actual.MinAllowedAge.Should().Be(expected.MinAllowedAge);
        actual.MaxAllowedAge.Should().Be(expected.MaxAllowedAge);
        actual.IsPublic.Should().Be(expected.IsPublic);
        actual.StartDate.Should().Be(expected.StartDate);
        actual.ReservationEndDate.Should().Be(expected.ReservationEndDate);
        actual.IsActive.Should().BeTrue();
        actual.UserId.Should().Be(TestUsers.DefaultUserId);

        // Address
        actual.EventAddress.Should().NotBeNull();
        actual.EventAddress.City.Should().Be(expected.City);
        actual.EventAddress.StreetAddress.Should().Be(expected.StreetAddress);
        actual.EventAddress.PostalCode.Should().Be(expected.PostalCode);
        actual.EventAddress.Region.Should().Be(expected.Region);

        // Food details
        actual.EventFoodDetails.Should().NotBeNull();
        actual.EventFoodDetails.Name.Should().Be(expected.EventFoodDetails.Name);
        actual.EventFoodDetails.Ingredients.Should().Be(expected.EventFoodDetails.Ingredients);
        actual.EventFoodDetails.AdditionalFoodItems.Should().Be(expected.EventFoodDetails.AdditionalFoodItems);
    }
}