using Events.Application.Dtos;
using Events.Application.Dtos.Requests;
using Events.Domain.Entities;

namespace Events.Application.Contracts.Services;

public interface IEventService
{
    // Basic CRUD
    Task<EventDto> AddEvent(CreateEventRequestDto createEvent);
    Task<EventDto> GetEvent(Guid id);
    Task<List<EventDto>> GetAllEvents();
    Task<EventDto> UpdateEvent(Guid id, Action<Event> op);
    Task<EventDto> DeleteEvent(Guid id);
    
    // Event specific
    
}