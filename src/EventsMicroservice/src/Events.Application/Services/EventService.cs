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

    private async Task EnsureThatUserOwnsTheReview(Guid reviewId)
    {
        var ec = await eventRepository.GetEventReviewById(reviewId);
        if (ec.UserId != currentUser.UserId)
        {
            throw new UnauthorizedUserForReviewException(ec.Id, ec.UserId, currentUser.UserId);
        }   
    }

    public async Task<EventDto> AddEvent(CreateEventRequestDto createEvent)
    {
        var eventEntity = createEvent.ToEntity(currentUser.UserId);
        var e = await eventRepository.AddEvent(eventEntity);
        
        createEvent.Images.Select(async image => await eventRepository.AddImageToEvent(e.EventId, image));

        return e.ToDto();
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
    
    public async Task<List<EventDto>> GetAllEvents()
    {
        _ = currentUser.UserId;
        var events = await eventRepository.GetAllEvents();
        return events
            .Where(e => e.IsActive)
            .ToList();
    }

    public async Task<EventDto> UpdateEvent(Guid eventId, UpdateEventRequestDto updateReq)
    {
        await EnsureThatUserOwnsTheEvent(eventId);
        return await eventRepository.UpdateEvent(eventId, @event =>
        {
            @event.Name = updateReq.Name;
            @event.Description = updateReq.Description;
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
        
        var e = await eventRepository.DeleteEvent(eventId);
        await eventRepository.RemoveAllImagesFromEvent(eventId);
        
        return e.ToDto();
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

    public async Task<EventReviewDto> CreateReview(Guid eventId, CreateEventReviewRequestDto createReq)
    {
        Guid userId = currentUser.UserId;
        var e = await eventRepository.GetEventById(eventId);
        var participants = await eventRepository.GetEventParticipants(eventId);

        if (!participants.Any(id => id == userId) || e.IsActive)
        {
            throw new CannotReviewException(eventId);
        }

        var reviewEntity = createReq.ToEntity(eventId, userId);
        await eventRepository.AddEventReview(reviewEntity);
        return reviewEntity.ToDto();
    }

    public async Task<EventReviewDto> DeleteReview(Guid reviewId)
    {
        await EnsureThatUserOwnsTheReview(reviewId);
        var review = await eventRepository.DeleteEventReview(reviewId);
        return review.ToDto();
    }

    public async Task<EventReviewDto> EditReview(Guid reviewId, UpdateEventReviewRequestDto updateReq)
    {
        await EnsureThatUserOwnsTheReview(reviewId);
        var updatedReview = await eventRepository.UpdateEventReview(reviewId, review =>
        {
            review.ReviewStars = updateReq.Stars;
            review.ReviewComment = updateReq.Comment;
        });
        return updatedReview.ToDto();
    }
    

    public async Task<List<EventReviewDto>> GetAllReviewsForAnEvent(Guid eventId)
    {
        var e = await eventRepository.GetEventById(eventId);
        return e.EventReviews
            .Select(ec => ec.ToDto())
            .ToList();
    }

    public async Task<EventImageDto> AddEventImage(Guid eventId, AddEventImageRequestDto createReq)
    {
        await EnsureThatUserOwnsTheEvent(currentUser.UserId);
        
        var image = createReq.ToEntity(eventId);
        await eventRepository.AddImageToEvent(eventId, image);
        return  image.ToDto();
    }

    public async Task<EventImageDto> RemoveEventImage(Guid imageId)
    {
        await EnsureThatUserOwnsTheEvent(currentUser.UserId);
        var image = await eventRepository.RemoveImageFromEvent(imageId)
            ?? throw new ImageNotFoundException(imageId);
        return image.ToDto();
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