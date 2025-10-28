using Events.Application.Dtos;
using Events.Application.Dtos.Requests;
using Events.Domain.Entities;

namespace Events.Application.Contracts.Repositories;

public interface IEventRepository
{
    Task<Event> GetEventById(Guid id);
    Task<List<EventDto>> GetAllEvents();
    Task<EventDto> AddEvent(Event createEvent);
    Task<EventDto> UpdateEvent(Guid id, Action<Event> op);
    Task<EventDto> DeleteEvent(Guid id);
    Task<List<EventDto>> GetEventsByUserId(Guid userId);
    
    // For participants
    Task<Event> AddEventParticipant(Guid eventId, Guid userId);
    Task<Event> RemoveEventParticipant(Guid eventId, Guid userId);
    Task<List<Guid>> GetEventParticipants(Guid eventId);
    Task<bool> IsUserParticipant(Guid eventId, Guid userId);
    
    // For comments
    Task<EventComment> AddEventComment(EventComment comment);
    Task<EventComment> UpdateEventComment(Guid commentId, Action<EventComment> op);
    Task<EventComment> DeleteEventComment(Guid commentId);
    Task<EventComment> GetComment(Guid commentId);
    Task<List<EventComment>> GetEventComments(Guid eventId);
}
