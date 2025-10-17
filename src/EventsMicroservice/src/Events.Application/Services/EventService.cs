using Events.Application.Contracts.Repositories;
using Events.Application.Contracts.Services;
using Events.Application.Dtos;
using Events.Application.Dtos.Requests;
using Events.Domain.Entities;
using Events.Application.Exceptions;

namespace Events.Application.Services;

public class EventService(IEventRepository eventRepository) : IEventService
{
    public Task<EventDto> AddEvent(bool isUserLoggedIn, CreateEventRequestDto createEvent) =>
        isUserLoggedIn
            ? eventRepository.AddEvent(createEvent)
            : throw new UnauthorizedAccessException($"You need to be logged in to create an event");

    public Task<EventDto> GetEventByEventId(bool isUserLoggedIn, Guid eventId) =>
        isUserLoggedIn 
            ? eventRepository.GetEventById(eventId)
            : throw new UnauthorizedAccessException($"You need to be logged in to view an event");
    

    public Task<List<EventDto>> GetEventsByUserId(bool isUserLoggedIn, Guid userId) =>
        isUserLoggedIn
            ? eventRepository.GetEventsByUserId(userId)
            : throw new UnauthorizedAccessException($"You need to be logged in to view all the events for user with id: {userId}");
    

    public Task<List<EventDto>> GetAllEvents()
    {
        return eventRepository.GetAllEvents();
    }

    // UserId getting carried into from the auth service
    public async Task<EventDto> UpdateEvent(Guid currentUserId, Guid eventId, UpdateEventRequestDto updateReq)
    {
        var e =  await eventRepository.GetEventById(eventId);
        if(e.UserId != currentUserId)
        {
            throw new UnauthorizedUserException(e.EventId, e.UserId, currentUserId);
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

    public async Task<EventDto> DeleteEvent(Guid currentUserId, Guid eventId)
    {
        var e =  await eventRepository.GetEventById(eventId);
        if(e.UserId != currentUserId)
        {
            throw new UnauthorizedUserException(e.EventId, e.UserId, currentUserId);
        } 
        return await eventRepository.DeleteEvent(eventId);
    }

    public async Task<EventDto> MakeEventPrivate(Guid currentUserId, Guid eventId)
    {
        var e =  await eventRepository.GetEventById(eventId);
        if(e.UserId != currentUserId)
        {
            throw new UnauthorizedUserException(e.EventId, e.UserId, currentUserId);
        }

        if (!e.IsPublic)
        {
            return e;
        }

        return await eventRepository.UpdateEvent(eventId, @event => @event.IsPublic = false);
    }

    public async Task<EventDto> MakeEventPublic(Guid currentUserId, Guid eventId)
    {
        var e =  await eventRepository.GetEventById(eventId);
        if(e.UserId != currentUserId)
        {
            throw new UnauthorizedUserException(e.EventId, e.UserId, currentUserId);
        }

        if (e.IsPublic)
        {
            return e;
        }
        
        return await eventRepository.UpdateEvent(eventId, @event => @event.IsPublic = false);
    }

    // TODO Check if participants have already paid
    public async Task<EventDto> CancelEvent(Guid currentUserId, Guid eventId)
    {
        var e = await eventRepository.GetEventById(eventId);
        if (e.UserId != currentUserId)
        {
            throw new UnauthorizedUserException(e.EventId, e.UserId, currentUserId);
        }
        
        return await eventRepository.UpdateEvent(eventId, @event => @event.IsActive = false);
    }
}