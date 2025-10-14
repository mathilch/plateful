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
    Task<List<EventDto>> GetEventByUserId(Guid userId);
}
