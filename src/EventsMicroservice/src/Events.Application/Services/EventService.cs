using System.Runtime.InteropServices.ComTypes;
using Events.Application.Contracts.Repositories;
using Events.Application.Contracts.Services;
using Events.Application.Dtos;
using Events.Application.Dtos.Requests;
using Events.Application.Exceptions;
using Events.Application.Mappers;
using Events.Domain.Entities;

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

    private async Task EnsureThatUserOwnsTheComment(Guid commentId)
    {
        var ec = await eventRepository.GetComment(commentId);
        if (ec.UserId != currentUser.UserId)
        {
            throw new UnauthorizedUserForCommentException(ec.Id, ec.UserId, currentUser.UserId);
        }   
    }

    public Task<EventDto> AddEvent(CreateEventRequestDto createEvent)
    {
        var eventEntity = createEvent.ToEntity(currentUser.UserId);
        return eventRepository.AddEvent(eventEntity);
    }

    public async Task<EventDto> GetEventByEventId(Guid eventId)
    {
        var e = await eventRepository.GetEventById(eventId);
        return e.ToDto();
    }

    public async Task<List<EventDto>> GetEventsByUserId(Guid loggedInUserId)
    {
        var events = await eventRepository.GetEventsByUserId(loggedInUserId);
        return events
            .Where(e => e.IsPublic ||
                        e.EventParticipants.Any(pe => pe.UserId == loggedInUserId))
            .ToList();
    }

    // TODO Make sure that the events are active
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
            return e.ToDto();
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
            return e.ToDto();
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
        await eventRepository.AddEventParticipant(eventId, currentUser.UserId);
        return e.ToDto();
    }

    public async Task<EventDto> WithdrawFromEvent(Guid eventId)
    {
        if (!await eventRepository.IsUserParticipant(eventId, currentUser.UserId))
        {
            throw new UserIsNotSignedUpForEventException(currentUser.UserId, eventId);
        }

        var e = await eventRepository.RemoveEventParticipant(eventId, currentUser.UserId);
        return e.ToDto();
    }

    // TODO should everybody be able to see who's signed up for an event?
    public async Task<List<Guid>> ViewEventParticipants(Guid eventId)
    {
        return await eventRepository.GetEventParticipants(eventId);
    }

    public async Task<EventCommentDto> CreateComment(Guid eventId, CreateEventCommentRequestDto createReq)
    {
        Guid userId = currentUser.UserId;
        var e = await eventRepository.GetEventById(eventId);
        var participants = await eventRepository.GetEventParticipants(eventId);

        if (!participants.Any(id => id == userId) || e.IsActive)
        {
            throw new CannotCommentException(eventId);
        }

        var commentEntity = createReq.ToEntity(eventId, userId);
        await eventRepository.AddEventComment(commentEntity);
        return commentEntity.ToDto();
    }

    public async Task<EventCommentDto> DeleteComment(Guid commentId)
    {
        await EnsureThatUserOwnsTheComment(commentId);
        var comment = await eventRepository.DeleteEventComment(commentId);
        return comment.ToDto();
    }

    public async Task<EventCommentDto> EditComment(Guid commentId, UpdateEventCommentRequestDto updateReq)
    {
        await EnsureThatUserOwnsTheComment(commentId);
        var updatedComment = await eventRepository.UpdateEventComment(commentId, comment => comment.Comment = updateReq.Comment);
        return updatedComment.ToDto();
    }
    

    public async Task<List<EventCommentDto>> GetAllCommentsForAnEvent(Guid eventId)
    {
        var e = await eventRepository.GetEventById(eventId);
        return e.EventComments
            .Select(ec => ec.ToDto())
            .ToList();
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