using Events.Application.Contracts.Repositories;
using Events.Application.Dtos;
using Events.Application.Dtos.Requests;
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

    public async Task<EventDto?> GetEventById(Guid id)
    {
        var e = await _context.Events.FindAsync(id);
        return e.ToDto();
    }

    public async Task<List<EventDto?>> GetAllEvents()
    {
        return await _context.Events
            .Select(e => e.ToDto())
            .ToListAsync();
    }

    public async Task<EventDto?> AddEvent(CreateEventRequestDto createEvent)
    {
        var e = new Event
        {
            EventId = Guid.NewGuid(),
            UserId = createEvent.UserId,
            Name = createEvent.Name,
            Description = createEvent.Description,
            FoodName = createEvent.FoodName,
            MaxAllowedParticipants = createEvent.MaxAllowedParticipants,
            MinAllowedAge = createEvent.MinAllowedAge,
            MaxAllowedAge = createEvent.MaxAllowedAge,
            StartDate = createEvent.StartDate,
            ReservationEndDate = createEvent.ReservationEndDate,
            ImageThumbnail = createEvent.ImageThumbnail,
            IsActive = true
        };

        _context.Events.Add(e);
        await _context.SaveChangesAsync();
        return e.ToDto();
    }

    public async Task<EventDto?> UpdateEvent(Guid id, Action<Event> op)
    {
        var e = await _context.Events.FindAsync(id);
        if (e is null) return null;

        op(e);
        await _context.SaveChangesAsync();
        return e.ToDto();
    }

     public async Task<EventDto?> DeleteEvent(Guid id)
    {
        var e = await _context.Events.FindAsync(id);
        if (e is null) return null;

        _context.Events.Remove(e);
        await _context.SaveChangesAsync();
        return e.ToDto();
    }

    public async Task<List<EventDto?>> GetByUserId(Guid userId)
    {
        return await _context.Events
            .Where(e => e.UserId == userId)
            .Select(e => e.ToDto())
            .ToListAsync();
    }
}