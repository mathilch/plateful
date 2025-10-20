using Events.Application.Dtos;
using Events.Application.Dtos.Requests;
using Events.Domain.Entities;

namespace Events.Application.Contracts.Repositories;

public interface IEventRepository
{
    Task<EventDto> GetEventById(Guid id);
    Task<List<EventDto>> GetAllEvents();
    Task<EventDto> AddEvent(CreateEventRequestDto createEvent);
    Task<EventDto> UpdateEvent(Guid id, Action<Event> op);
    Task<EventDto> DeleteEvent(Guid id);
    Task<List<EventDto>> GetEventsByUserId(Guid userId);
    
    // For participants
    Task<EventDto> AddEventParticipant(Guid eventId, Guid userId);
    Task<EventDto> RemoveEventParticipant(Guid eventId, Guid userId);
    
    //questionable? should it return a list of userDto instead
    Task<List<Guid>> GetEventParticipants(Guid eventId);
}
