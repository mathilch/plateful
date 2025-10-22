using Events.Application.Contracts.Repositories;
using Events.Application.Contracts.Services;
using Events.Application.Dtos;
using Events.Application.Dtos.Requests;
using Events.Application.Exceptions;

namespace Events.Application.Services;

public class EventService(IEventRepository eventRepository) : IEventService
{

    // Helper methods to avoid code duplication
    private void EnsureThatUserIsLoggedIn(Guid loggedInUserId)
    {
        if (loggedInUserId == Guid.Empty)
        {
            throw new UnauthorizedAccessException($"You need to be logged in to create an event");
        }
    }
    
    // Helper methods to avoid code duplication
    private async Task EnsureThatUserOwnsTheEvent(Guid loggedInUserId, Guid eventId)
    {
        var e =  await eventRepository.GetEventById(eventId);
        if(e.UserId != loggedInUserId)
        {
            throw new UnauthorizedUserException(e.EventId, e.UserId, loggedInUserId);
        } 
    }
    
    public Task<EventDto> AddEvent(Guid loggedInUserId, CreateEventRequestDto createEvent)
    {
        EnsureThatUserIsLoggedIn(loggedInUserId);
        return eventRepository.AddEvent(createEvent with { UserId = loggedInUserId });
    }

    public Task<EventDto> GetEventByEventId(Guid loggedInUserId, Guid eventId)
    {
        EnsureThatUserIsLoggedIn(loggedInUserId);
        return eventRepository.GetEventById(eventId);
    }
    
    public async Task<List<EventDto>> GetEventsByUserId(Guid loggedInUser, Guid loggedInUserId)
    {
        EnsureThatUserIsLoggedIn(loggedInUserId);
        var events = await eventRepository.GetEventsByUserId(loggedInUserId);
        return events
            .Where(e => e.IsPublic ||
                        e.EventParticipants.Any(pe => pe.UserId == loggedInUserId))
            .ToList();
    }

    public Task<List<EventDto>> GetAllEvents()
    {
        return eventRepository.GetAllEvents();
    }
    
    public async Task<EventDto> UpdateEvent(Guid loggedInUserId, Guid eventId, UpdateEventRequestDto updateReq)
    {
        await EnsureThatUserOwnsTheEvent(loggedInUserId, eventId);
        return await eventRepository.UpdateEvent(eventId, @event =>
        {
            @event.Name = updateReq.Name;
            @event.Description = updateReq.Description;
            @event.FoodName =  updateReq.FoodName;
            @event.MaxAllowedParticipants = updateReq.MaxAllowedParticipants;
            @event.MinAllowedAge = updateReq.MinAllowedAge;
            @event.MaxAllowedAge = updateReq.MaxAllowedAge;
            @event.StartDate = updateReq.StartDate;
            @event.ReservationEndDate = updateReq.ReservationEndDate;
            @event.ImageThumbnail = updateReq.ImageThumbnailUrl;
        });
    }

    public async Task<EventDto> DeleteEvent(Guid loggedInUserId, Guid eventId)
    {
        await EnsureThatUserOwnsTheEvent(loggedInUserId, eventId);
        return await eventRepository.DeleteEvent(eventId);
    }

    public async Task<EventDto> MakeEventPrivate(Guid currentloggedInUserId, Guid eventId)
    {
        var e =  await eventRepository.GetEventById(eventId);
        if(e.UserId != currentloggedInUserId)
        {
            throw new UnauthorizedUserException(e.EventId, e.UserId, currentloggedInUserId);
        }

        if (!e.IsPublic)
        {
            return e;
        }

        return await eventRepository.UpdateEvent(eventId, @event => @event.IsPublic = false);
    }

    public async Task<EventDto> MakeEventPublic(Guid loggedInUserId, Guid eventId)
    {
        var e =  await eventRepository.GetEventById(eventId);
        if(e.UserId != loggedInUserId)
        {
            throw new UnauthorizedUserException(e.EventId, e.UserId, loggedInUserId);
        }

        if (e.IsPublic)
        {
            return e;
        }
        
        return await eventRepository.UpdateEvent(eventId, @event => @event.IsPublic = false);
    }

    // TODO Check if participants have already paid
    public async Task<EventDto> CancelEvent(Guid loggedInUserId, Guid eventId)
    {
        await EnsureThatUserOwnsTheEvent(loggedInUserId, eventId);
        return await eventRepository.UpdateEvent(eventId, @event => @event.IsActive = false);
    }
    
    public async Task<EventDto> SignUpForEvent(Guid loggedInUserId, Guid eventId, DateOnly userBirthday)
    {
        EnsureThatUserIsLoggedIn(loggedInUserId);
        var e = await eventRepository.GetEventById(eventId);
        var userAge = CalculateAge(userBirthday);
        if (userAge < e.MinAllowedAge || userAge > e.MaxAllowedAge)
        {
            throw new UserIsNotTheRightAgeException(loggedInUserId, e.MinAllowedAge, e.MaxAllowedAge);
        }
        return await eventRepository.AddEventParticipant(eventId, loggedInUserId);
    }

    public async Task<EventDto> WithdrawFromEvent(Guid loggedInUserId, Guid eventId)
    {
        EnsureThatUserIsLoggedIn(loggedInUserId);
        if (!await eventRepository.IsUserParticipant(eventId, loggedInUserId))
        {
            throw new UserIsNotSignedUpForEventException(loggedInUserId, eventId);
        }

        return await eventRepository.RemoveEventParticipant(eventId, loggedInUserId);
    }

    // TODO should everybody be able to see who's signed up for an event?
    public async Task<List<Guid>> ViewEventParticipants(Guid loggedInUserId, Guid eventId)
    {
        EnsureThatUserIsLoggedIn(loggedInUserId);
        return await eventRepository.GetEventParticipants(eventId);
    }



    private static int CalculateAge(DateOnly birthday)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        int age = today.Year - birthday.Year;

        if (birthday > today.AddYears(-age))
        {
            age--;
        }
        return age;   
    }
    
}