using Events.Application.Contracts.Repositories;
using Events.Application.Dtos;
using Events.Application.Exceptions;
using Events.Application.Mappers;
using Events.Domain.Entities;
using Events.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Repository;

public class EventRepository : IEventRepository
{
    private readonly EventsDbContext _context;

    public EventRepository(EventsDbContext context)
    {
        _context = context;
    }

    public async Task<Event> GetEventById(Guid id)
    {
        var e = await _context.Events.FindAsync(id)
            ?? throw new EventIdNotFoundException(id);
        return e;
    }

    public async Task<List<EventDto>> GetAllEvents()
    {
        return await _context.Events
            .Select(e => e.ToDto())
            .ToListAsync();
    }

    public async Task<Event> AddEvent(Event newEvent)
    {
        newEvent.EventId = Guid.NewGuid();
        _context.Events.Add(newEvent);
        await _context.SaveChangesAsync();
        return newEvent;
    }

    public async Task<EventDto> UpdateEvent(Guid id, Action<Event> op)
    {
        var e = await _context.Events.FindAsync(id)
                ?? throw new EventIdNotFoundException(id);

        op(e);
        await _context.SaveChangesAsync();
        return e.ToDto();
    }

    public async Task<Event> DeleteEvent(Guid id)
    {
        var e = await _context.Events.FindAsync(id)
            ?? throw new EventIdNotFoundException(id);

        _context.Events.Remove(e);
        await _context.SaveChangesAsync();
        return e;
    }

    public async Task<List<EventDto>> GetEventsByUserId(Guid userId)
    {
        return await _context.Events
            .Where(e => e.UserId == userId)
            .Select(e => e.ToDto())
            .ToListAsync();
    }

    public async Task<Event> AddEventParticipant(Guid eventId, Guid userId)
    {
        var ep = new EventParticipant
        {
            Id = Guid.NewGuid(),
            EventId = eventId,
            UserId = userId,
            CreatedDate = DateTime.Now
        };

        _context.EventParticipants.Add(ep);
        await _context.SaveChangesAsync();
        return await GetEventById(eventId);
    }

    // Specific for EventParticipant
    public async Task<Event> RemoveEventParticipant(Guid eventId, Guid userId)
    {
        var ep = await _context.EventParticipants
            .FirstOrDefaultAsync(ep => ep.EventId == eventId && ep.UserId == userId)
            ?? throw new RemoveEventParticipantException(eventId, userId);

        _context.EventParticipants.Remove(ep);
        await _context.SaveChangesAsync();
        return await GetEventById(eventId);
    }

    public async Task<List<Guid>> GetEventParticipants(Guid eventId)
    {
        return await _context.EventParticipants
            .Select(ep => ep.UserId)
            .Where(ep => ep == eventId)
            .ToListAsync();
    }

    public async Task<bool> IsUserParticipant(Guid eventId, Guid userId)
    {
        return await _context.EventParticipants
            .AnyAsync(ep => ep.UserId == userId && ep.EventId == eventId);
    }
    
    // Specific for EventReviews
    public async Task<EventReview> AddEventReview(EventReview review)
    {
        review.Id = Guid.NewGuid();
        
        _context.EventReviews.Add(review);
        await _context.SaveChangesAsync();
        return review;
    }

    public async Task<EventReview> UpdateEventReview(Guid reviewId, Action<EventReview> op)
    {
        var er = await _context.EventReviews.FindAsync(reviewId)
                ?? throw new ReviewIdNotFoundException(reviewId);

        op(er);
        await _context.SaveChangesAsync();
        return er;
    }

    public async Task<EventReview> DeleteEventReview(Guid reviewId)
    {
        var er = await _context.EventReviews.FindAsync(reviewId)
                ?? throw new ReviewIdNotFoundException(reviewId);

        _context.EventReviews.Remove(er);
        await _context.SaveChangesAsync();
        return er;
    }

    public async Task<List<EventReview>> GetEventReviews(Guid eventId)
    {
        var reviews = await _context.Events
            .Where(e => e.EventId == eventId)
            .SelectMany(e => e.EventReviews)
            .ToListAsync();

        return reviews;
    }

    public async Task<EventReview> GetEventReviewById(Guid reviewId)
    {
        return await _context.EventReviews.FindAsync(reviewId)
            ?? throw new ReviewIdNotFoundException(reviewId);
    }

    public async Task<EventImage> AddImageToEvent(Guid eventId, EventImage image)
    {
        image.Id = Guid.NewGuid();
        image.EventId = eventId;
        _context.EventImages.Add(image);
        await _context.SaveChangesAsync();
        return image;
    }

    public async Task<EventImage> RemoveImageFromEvent(Guid imageId)
    {
        var ei = await _context.EventImages.FindAsync(imageId)
            ?? throw new ImageNotFoundException(imageId);
        _context.EventImages.Remove(ei);
        await _context.SaveChangesAsync();
        return ei;
    }

    public async Task<List<EventImage>> RemoveAllImagesFromEvent(Guid eventId)
    {
        var images = _context.EventImages
            .Where(image => image.EventId == eventId)
            .ToList();

        if (images.Any())
        {
            _context.EventImages.RemoveRange(images);
            await _context.SaveChangesAsync();
        }

        return images;
    }

    public async Task<EventFoodDetails> AddEventFoodDetails(Guid eventId, EventFoodDetails foodDetails)
    {
        foodDetails.Id = Guid.NewGuid();
        foodDetails.EventId = eventId;
        _context.EventFoodDetails.Add(foodDetails);
        await _context.SaveChangesAsync();
        return foodDetails;
    }

    public async Task<EventFoodDetails> UpdateEventFoodDetails(Guid eventId, Action<EventFoodDetails> op)
    {
        var foodDetails = await
                              _context.EventFoodDetails.FirstOrDefaultAsync(fd => fd.EventId == eventId)
                          ?? throw new Exception();

        op(foodDetails);
        await _context.SaveChangesAsync();
        return foodDetails;
    }

    public Task<EventFoodDetails> GetEventFoodDetails(Guid eventId)
    {
        throw new NotImplementedException();
    }

    public Task<EventFoodDetails> RemoveEventFoodDetails(Guid eventId)
    {
        throw new NotImplementedException();
    }
}