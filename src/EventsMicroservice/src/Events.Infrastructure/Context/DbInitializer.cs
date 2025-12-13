using Events.Domain.Entities;
using Events.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Context;

public static class DbInitializer
{
    public static void Seed(EventsDbContext context)
    {
        context.Database.Migrate();
        
        if (!context.Events.Any())
        {
            var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var mathiasId = Guid.Parse("019b17fc-5147-794b-8a08-bfa49a2aaa90");
            var testUserId = Guid.Parse("019b1802-a063-7095-9e29-8f9f3df736a1");
            
            var e1Id = Guid.NewGuid();

            var ep1 = new EventParticipant
            {
                Id = Guid.NewGuid(),
                EventId = e1Id,
                UserId = testUserId,
                CreatedDate = DateTime.UtcNow,
                ParticipantStatus = ParticipantStatus.Approved,
                PaymentStatus = PaymentStatus.Paid
            };
            
            var fd1 = new EventFoodDetails
            {
                DietaryStyles = [],
                Allergens = ["Gluten, Dairy"],
                Name = "Pizza",
                Ingredients = "Flour, water, meat, cheese",
                AdditionalFoodItems = "Monster Energy Nitro"
            };

            var ea1 = new EventAddress
            {
                StreetAddress = "Asmild Ager 2",
                PostalCode = "8800",
                City = "Viborg",
                Region = "Region Midtjylland"
            };
            
            var e1 = new Event
            {
                EventId = e1Id,
                UserId = mathiasId,
                Name = "Cozy Candlelit Dinner",
                Description = "Freshly made",
                MaxAllowedParticipants = 5,
                PricePerSeat = 30,
                MinAllowedAge = 18,
                MaxAllowedAge = 25,
                StartDate = new DateTime(2025, 12, 24, 18, 0, 0, DateTimeKind.Utc),
                ReservationEndDate = new DateTime(2025, 12, 24, 16, 0, 0, DateTimeKind.Utc),
                ImageThumbnail = "",
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                IsPublic = true,
                EventFoodDetails = fd1,
                EventAddress = ea1,
                EventParticipants = [ep1]
            };   
            

            // Event 2
            var e2Id = Guid.NewGuid();
            var fd2 = new EventFoodDetails
            {
                DietaryStyles = [],
                Allergens = [],
                Name = "Sushi Platter",
                Ingredients = "Rice, fish, seaweed, wasabi",
                AdditionalFoodItems = "Green Tea"
            };

            var e2 = new Event
            {
                EventId = e2Id,
                UserId = testUserId,
                Name = "Japanese Sushi Night",
                Description = "Fresh sushi made to order",
                MaxAllowedParticipants = 10,
                PricePerSeat = 50,
                MinAllowedAge = 21,
                MaxAllowedAge = 35,
                StartDate = new DateTime(2026, 1, 15, 19, 30, 0, DateTimeKind.Utc),
                ReservationEndDate = new DateTime(2026, 1, 15, 22, 0, 0, DateTimeKind.Utc),
                ImageThumbnail = "",
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                IsPublic = true,
                EventFoodDetails = fd2
            };

            // Event 3
            var e3Id = Guid.NewGuid();
            var fd3 = new EventFoodDetails
            {
                DietaryStyles = [],
                Allergens = [],
                Name = "BBQ Feast",
                Ingredients = "Beef, chicken, sausages, veggies",
                AdditionalFoodItems = "Lemonade, Beer"
            };

            var e3 = new Event
            {
                EventId = e3Id,
                UserId = testUserId,
                Name = "Summer BBQ Bash",
                Description = "Outdoor grilling and fun",
                MaxAllowedParticipants = 20,
                PricePerSeat = 15,
                MinAllowedAge = 18,
                MaxAllowedAge = 40,
                StartDate = new DateTime(2026, 6, 20, 16, 0, 0, DateTimeKind.Utc),
                ReservationEndDate = new DateTime(2026, 6, 20, 23, 0, 0, DateTimeKind.Utc),
                ImageThumbnail = "",
                CreatedDate = DateTime.UtcNow,
                IsActive = true,
                IsPublic = true,
                EventFoodDetails = fd3
            };

            // Add events and food details to context
            context.Events.AddRange(e1, e2, e3);
            context.EventFoodDetails.AddRange(fd1, fd2, fd3);
            context.EventParticipants.AddRange(ep1);
            context.SaveChanges();
        }
    }
}