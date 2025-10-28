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
    
    /* ===========================================================================
     * =============== P A R T I C I P A N T S ===================================
     * ===========================================================================
     */
    Task<Event> AddEventParticipant(Guid eventId, Guid userId);
    Task<Event> RemoveEventParticipant(Guid eventId, Guid userId);
    Task<List<Guid>> GetEventParticipants(Guid eventId);
    Task<bool> IsUserParticipant(Guid eventId, Guid userId);
    
    
    /* ===========================================================================
     * ======================= R E V I E W S =====================================
     * ===========================================================================
     */
    Task<EventReview> AddEventReview(EventReview review);
    Task<EventReview> UpdateEventReview(Guid commentId, Action<EventReview> op);
    Task<EventReview> DeleteEventReview(Guid commentId);
    Task<List<EventReview>> GetEventReviews(Guid eventId);
    Task<EventReview> GetEventReviewById(Guid reviewId);
}
