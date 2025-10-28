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

    public async Task<EventDto> AddEvent(Event newEvent)
    {
        newEvent.EventId = Guid.NewGuid();
        _context.Events.Add(newEvent);
        await _context.SaveChangesAsync();
        return newEvent.ToDto();
    }

    public async Task<EventDto> UpdateEvent(Guid id, Action<Event> op)
    {
        var e = await _context.Events.FindAsync(id)
                ?? throw new EventIdNotFoundException(id);

        op(e);
        await _context.SaveChangesAsync();
        return e.ToDto();
    }

    public async Task<EventDto> DeleteEvent(Guid id)
    {
        var e = await _context.Events.FindAsync(id)
            ?? throw new EventIdNotFoundException(id);

        _context.Events.Remove(e);
        await _context.SaveChangesAsync();
        return e.ToDto();
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
}