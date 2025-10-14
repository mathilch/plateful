using Events.Application.Contracts.Repositories;
using Events.Application.Contracts.Services;
using Events.Application.Dtos;
using Events.Application.Dtos.Requests;
using Events.Domain.Entities;

namespace Events.Application.Services;

public class EventService(IEventRepository _eventRepository) : IEventService
{
    public Task<EventDto> AddEvent(CreateEventRequestDto createEvent)
    {
        throw new NotImplementedException();
    }

    public Task<EventDto> GetEvent(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<List<EventDto>> GetAllEvents()
    {
        throw new NotImplementedException();
    }

    public Task<EventDto> UpdateEvent(Guid id, Action<Event> op)
    {
        throw new NotImplementedException();
    }

    public Task<EventDto> DeleteEvent(Guid id)
    {
        throw new NotImplementedException();
    }
}