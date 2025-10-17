using Events.Application.Dtos;
using Events.Application.Dtos.Requests;
using Events.Domain.Entities;

namespace Events.Application.Contracts.Services;

public interface IEventService
{
    // Basic CRUD
    Task<EventDto> AddEvent(Guid loggedInUserId, CreateEventRequestDto createEvent);
    Task<EventDto> GetEventByEventId(Guid loggedInUserId, Guid eventId);
    Task<List<EventDto>> GetEventsByUserId(Guid loggedInUserId, Guid userId);
    Task<List<EventDto>> GetAllEvents();
    Task<EventDto> UpdateEvent(Guid loggedInUserId, Guid eventId, UpdateEventRequestDto updateReq);
    Task<EventDto> DeleteEvent(Guid loggedInUserId, Guid eventId);
    
    // Event specific
    Task<EventDto> MakeEventPrivate(Guid loggedInUserId, Guid eventId);
    Task<EventDto> MakeEventPublic(Guid loggedInUserId, Guid eventId);
    Task<EventDto> CancelEvent(Guid loggedInUserId, Guid eventId);

}