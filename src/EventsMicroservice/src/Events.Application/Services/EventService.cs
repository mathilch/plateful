using Events.Application.Contracts.Repositories;
using Events.Application.Contracts.Services;
using Events.Application.Dtos;
using Events.Application.Dtos.Requests;
using Events.Application.Exceptions;

namespace Events.Application.Services;

public class EventService(IEventRepository eventRepository) : IEventService
{
    public Task<EventDto> AddEvent(Guid loggedInUserId, CreateEventRequestDto createEvent)
    {
        if (loggedInUserId == Guid.Empty)
        {
            throw new UnauthorizedAccessException($"You need to be logged in to create an event");
        }
        return eventRepository.AddEvent(createEvent with { UserId = loggedInUserId });
    }

    public Task<EventDto> GetEventByEventId(Guid loggedInUserId, Guid eventId)
    {
        if (loggedInUserId == Guid.Empty)
        {
            throw new UnauthorizedAccessException($"You need to be logged in to view an event");
        }

        return eventRepository.GetEventById(eventId);
    }

    // TODO Also retreive events where the loggedInUser exists in the participation list.
    // Need to factor Event Entity to contain a list of participants
    public async Task<List<EventDto>> GetEventsByUserId(Guid loggedInUser, Guid loggedInUserId)
    {
        if (loggedInUserId == Guid.Empty)
        {
            throw new UnauthorizedAccessException($"You need to be logged in to view all the events for user with id: {loggedInUserId}");
        }
        var events = await eventRepository.GetEventsByUserId(loggedInUserId);
        return events
            .Where(e => e.IsPublic)
            .ToList();
    }

    public Task<List<EventDto>> GetAllEvents()
    {
        return eventRepository.GetAllEvents();
    }

    // loggedInUserId getting carried into from the auth service
    public async Task<EventDto> UpdateEvent(Guid currentloggedInUserId, Guid eventId, UpdateEventRequestDto updateReq)
    {
        var e =  await eventRepository.GetEventById(eventId);
        if(e.UserId != currentloggedInUserId)
        {
            throw new UnauthorizedUserException(e.EventId, e.UserId, currentloggedInUserId);
        } 
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

    public async Task<EventDto> DeleteEvent(Guid currentloggedInUserId, Guid eventId)
    {
        var e =  await eventRepository.GetEventById(eventId);
        if(e.UserId != currentloggedInUserId)
        {
            throw new UnauthorizedUserException(e.EventId, e.UserId, currentloggedInUserId);
        } 
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

    public async Task<EventDto> MakeEventPublic(Guid currentloggedInUserId, Guid eventId)
    {
        var e =  await eventRepository.GetEventById(eventId);
        if(e.UserId != currentloggedInUserId)
        {
            throw new UnauthorizedUserException(e.EventId, e.UserId, currentloggedInUserId);
        }

        if (e.IsPublic)
        {
            return e;
        }
        
        return await eventRepository.UpdateEvent(eventId, @event => @event.IsPublic = false);
    }

    // TODO Check if participants have already paid
    public async Task<EventDto> CancelEvent(Guid currentloggedInUserId, Guid eventId)
    {
        var e = await eventRepository.GetEventById(eventId);
        if (e.UserId != currentloggedInUserId)
        {
            throw new UnauthorizedUserException(e.EventId, e.UserId, currentloggedInUserId);
        }
        
        return await eventRepository.UpdateEvent(eventId, @event => @event.IsActive = false);
    }
}