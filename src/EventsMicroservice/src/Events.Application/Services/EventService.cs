using Events.Application.Contracts.Repositories;
using Events.Application.Contracts.Services;
using Events.Application.Dtos;
using Events.Application.Dtos.Requests;
using Events.Application.Exceptions;

namespace Events.Application.Services;

public class EventService(IEventRepository eventRepository, CurrentUser currentUser) : IEventService
{
    // Helper methods to avoid code duplication
    private async Task EnsureThatUserOwnsTheEvent(Guid eventId)
    {
        var e = await eventRepository.GetEventById(eventId);
        if (e.UserId != currentUser.UserId)
        {
            throw new UnauthorizedUserException(e.EventId, e.UserId, currentUser.UserId);
        }
    }

    public Task<EventDto> AddEvent(CreateEventRequestDto createEvent)
    {
        return eventRepository.AddEvent(createEvent with { UserId = currentUser.UserId });
    }

    public Task<EventDto> GetEventByEventId(Guid eventId)
    {
        return eventRepository.GetEventById(eventId);
    }

    public async Task<List<EventDto>> GetEventsByUserId(Guid loggedInUserId)
    {
        var events = await eventRepository.GetEventsByUserId(loggedInUserId);
        return events
            .Where(e => e.IsPublic ||
                        e.EventParticipants.Any(pe => pe.UserId == loggedInUserId))
            .ToList();
    }

    public Task<List<EventDto>> GetAllEvents()
    {
        Guid userId = currentUser.UserId;
        return eventRepository.GetAllEvents();
    }

    public async Task<EventDto> UpdateEvent(Guid eventId, UpdateEventRequestDto updateReq)
    {
        await EnsureThatUserOwnsTheEvent(eventId);
        return await eventRepository.UpdateEvent(eventId, @event =>
        {
            @event.Name = updateReq.Name;
            @event.Description = updateReq.Description;
            @event.FoodName = updateReq.FoodName;
            @event.MaxAllowedParticipants = updateReq.MaxAllowedParticipants;
            @event.MinAllowedAge = updateReq.MinAllowedAge;
            @event.MaxAllowedAge = updateReq.MaxAllowedAge;
            @event.StartDate = updateReq.StartDate;
            @event.ReservationEndDate = updateReq.ReservationEndDate;
            @event.ImageThumbnail = updateReq.ImageThumbnailUrl;
        });
    }

    public async Task<EventDto> DeleteEvent(Guid eventId)
    {
        await EnsureThatUserOwnsTheEvent(eventId);
        return await eventRepository.DeleteEvent(eventId);
    }

    public async Task<EventDto> MakeEventPrivate(Guid eventId)
    {
        var e = await eventRepository.GetEventById(eventId);
        if (e.UserId != currentUser.UserId)
        {
            throw new UnauthorizedUserException(e.EventId, e.UserId, currentUser.UserId);
        }

        if (!e.IsPublic)
        {
            return e;
        }

        return await eventRepository.UpdateEvent(eventId, @event => @event.IsPublic = false);
    }

    public async Task<EventDto> MakeEventPublic(Guid eventId)
    {
        var e = await eventRepository.GetEventById(eventId);
        if (e.UserId != currentUser.UserId)
        {
            throw new UnauthorizedUserException(e.EventId, e.UserId, currentUser.UserId);
        }

        if (e.IsPublic)
        {
            return e;
        }

        return await eventRepository.UpdateEvent(eventId, @event => @event.IsPublic = false);
    }

    // TODO Check if participants have already paid
    public async Task<EventDto> CancelEvent(Guid eventId)
    {
        await EnsureThatUserOwnsTheEvent(eventId);
        return await eventRepository.UpdateEvent(eventId, @event => @event.IsActive = false);
    }

    public async Task<EventDto> SignUpForEvent(Guid eventId, DateOnly userBirthday)
    {
        var e = await eventRepository.GetEventById(eventId);
        var userAge = CalculateAge(userBirthday);
        if (userAge < e.MinAllowedAge || userAge > e.MaxAllowedAge)
        {
            throw new UserIsNotTheRightAgeException(currentUser.UserId, e.MinAllowedAge, e.MaxAllowedAge);
        }
        return await eventRepository.AddEventParticipant(eventId, currentUser.UserId);
    }

    public async Task<EventDto> WithdrawFromEvent(Guid eventId)
    {
        if (!await eventRepository.IsUserParticipant(eventId, currentUser.UserId))
        {
            throw new UserIsNotSignedUpForEventException(currentUser.UserId, eventId);
        }

        return await eventRepository.RemoveEventParticipant(eventId, currentUser.UserId);
    }

    // TODO should everybody be able to see who's signed up for an event?
    public async Task<List<Guid>> ViewEventParticipants(Guid eventId)
    {
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