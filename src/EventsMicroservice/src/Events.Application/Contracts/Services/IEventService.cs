using Events.Application.Dtos;
using Events.Application.Dtos.Requests;

namespace Events.Application.Contracts.Services;

public interface IEventService
{
    // Basic CRUD
    Task<EventDto> AddEvent(CreateEventRequestDto createEvent);
    Task<EventDto> GetEventByEventId(Guid eventId);
    Task<List<EventDto>> GetEventsByUserId(Guid userId);
    Task<List<EventDto>> GetAllEvents();
    Task<EventDto> UpdateEvent(Guid eventId, UpdateEventRequestDto updateReq);
    Task<EventDto> DeleteEvent(Guid eventId);
    
    // Event specific
    Task<EventDto> MakeEventPrivate(Guid eventId);
    Task<EventDto> MakeEventPublic(Guid eventId);
    Task<EventDto> CancelEvent(Guid eventId);
    
    // Event participation
    Task<EventDto> SignUpForEvent(Guid eventId, DateOnly userBirthday);
    Task<EventDto> WithdrawFromEvent(Guid eventId);
    Task<List<Guid>> ViewEventParticipants(Guid eventId);
    
    // Event Comments 
    Task<EventReviewDto> CreateReview(Guid eventId, CreateEventReviewRequestDto createReq);
    Task<EventReviewDto> DeleteReview(Guid reviewId);
    Task<EventReviewDto> EditReview(Guid reviewId, UpdateEventReviewRequestDto updateReq);
    Task<List<EventReviewDto>> GetAllReviewsForAnEvent(Guid eventId);
}