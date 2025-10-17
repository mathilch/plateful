using Events.Application.Dtos;
using Events.Application.Dtos.Requests;
using Events.Domain.Entities;

namespace Events.Application.Contracts.Services;

public interface IEventService
{
    // Basic CRUD
    Task<EventDto> AddEvent(bool isUserLoggedIn, CreateEventRequestDto createEvent);
    Task<EventDto> GetEventByEventId(bool isUserLoggedIn, Guid eventId);
    Task<List<EventDto>> GetEventsByUserId(bool isUserLoggedIn, Guid userId);
    Task<List<EventDto>> GetAllEvents();
    Task<EventDto> UpdateEvent(Guid userId, Guid eventId, UpdateEventRequestDto updateReq);
    Task<EventDto> DeleteEvent(Guid userId, Guid eventId);
    
    // Event specific
    Task<EventDto> MakeEventPrivate(Guid userId, Guid eventId);
    Task<EventDto> MakeEventPublic(Guid userId, Guid eventId);
    Task<EventDto> CancelEvent(Guid userId, Guid eventId);

}