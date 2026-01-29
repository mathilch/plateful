using Events.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Events.Infrastructure.Context;

public static class DbInitializer
{
    public static void Seed(EventsDbContext context)
    {
        context.Database.Migrate();

        if (!context.Events.Any())
        {
            var seedData = LoadSeedData();
            if (seedData == null)
            {
                Console.WriteLine("Warning: Could not load seed data from SeedData.json");
                return;
            }

            var events = new List<Event>();
            var foodDetailsList = new List<EventFoodDetails>();

            foreach (var eventData in seedData.Events)
            {
                var eventId = Guid.NewGuid();
                
                var foodDetails = new EventFoodDetails
                {
                    Id = Guid.NewGuid(),
                    EventId = eventId,
                    Name = eventData.FoodDetails.Name,
                    Ingredients = eventData.FoodDetails.Ingredients,
                    AdditionalFoodItems = eventData.FoodDetails.AdditionalFoodItems,
                    DietaryStyles = eventData.FoodDetails.DietaryStyles,
                    Allergens = eventData.FoodDetails.Allergens
                };

                var eventEntity = new Event
                {
                    EventId = eventId,
                    UserId = Guid.Parse(eventData.UserId),
                    Name = eventData.Name,
                    Description = eventData.Description,
                    MaxAllowedParticipants = eventData.MaxAllowedParticipants,
                    PricePerSeat = eventData.PricePerSeat,
                    MinAllowedAge = eventData.MinAllowedAge,
                    MaxAllowedAge = eventData.MaxAllowedAge,
                    StartDate = DateTime.UtcNow.AddDays(eventData.StartDateOffset),
                    ReservationEndDate = DateTime.UtcNow.AddDays(eventData.ReservationEndDateOffset),
                    ImageThumbnail = eventData.ImageThumbnail,
                    CreatedDate = DateTime.UtcNow,
                    IsActive = eventData.IsActive,
                    IsPublic = eventData.IsPublic,
                    EventFoodDetails = foodDetails,
                    EventAddress = new EventAddress
                    {
                        StreetAddress = eventData.Address.StreetAddress,
                        PostalCode = eventData.Address.PostalCode,
                        City = eventData.Address.City,
                        Region = eventData.Address.Region
                    }
                };

                events.Add(eventEntity);
                foodDetailsList.Add(foodDetails);
            }

            context.Events.AddRange(events);
            context.EventFoodDetails.AddRange(foodDetailsList);
            context.SaveChanges();
            
            Console.WriteLine($"Seeded {events.Count} events successfully.");
        }
    }

    private static SeedDataRoot? LoadSeedData()
    {
        var assembly = typeof(DbInitializer).Assembly;
        var resourceName = "Events.Infrastructure.Context.SeedData.json";
        
        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
        {
            // Fallback: try to load from file path
            var basePath = AppContext.BaseDirectory;
            var filePath = Path.Combine(basePath, "SeedData.json");
            
            if (!File.Exists(filePath))
            {
                // Try relative path from assembly location
                var assemblyPath = Path.GetDirectoryName(assembly.Location);
                filePath = Path.Combine(assemblyPath ?? basePath, "SeedData.json");
            }

            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<SeedDataRoot>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            
            return null;
        }

        using var reader = new StreamReader(stream);
        var content = reader.ReadToEnd();
        return JsonSerializer.Deserialize<SeedDataRoot>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }
}

#region Seed Data DTOs

public class SeedDataRoot
{
    public SeedUsers Users { get; set; } = new();
    public List<SeedEvent> Events { get; set; } = [];
}

public class SeedUsers
{
    public string MathiasId { get; set; } = string.Empty;
    public string TestUserId { get; set; } = string.Empty;
}

public class SeedEvent
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public int MaxAllowedParticipants { get; set; }
    public double PricePerSeat { get; set; }
    public int MinAllowedAge { get; set; }
    public int MaxAllowedAge { get; set; }
    public int StartDateOffset { get; set; }
    public int ReservationEndDateOffset { get; set; }
    public string ImageThumbnail { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool IsPublic { get; set; }
    public SeedFoodDetails FoodDetails { get; set; } = new();
    public SeedAddress Address { get; set; } = new();
}

public class SeedFoodDetails
{
    public string Name { get; set; } = string.Empty;
    public string Ingredients { get; set; } = string.Empty;
    public string AdditionalFoodItems { get; set; } = string.Empty;
    public List<string> DietaryStyles { get; set; } = [];
    public List<string> Allergens { get; set; } = [];
}

public class SeedAddress
{
    public string StreetAddress { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
}

#endregion