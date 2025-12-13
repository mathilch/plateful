using Events.Application.Contracts.ExternalApis;
using Events.Application.Contracts.Services;
using Events.Application.Dtos.Common;
using Events.Application.Dtos.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Events.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/event")]
public class EventController(IEventService _eventService, IPaymentService _stripeService) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("recent")]
    public async Task<IActionResult> GetRecentEvents()
    {
        return Ok(await _eventService.GetRecentEvents(new PaginationDto(0, 10)));
    }

    [AllowAnonymous]
    [HttpGet("search")]
    public async Task<IActionResult> GetFilteredEvents(SearchEventsRequestDto searchEventsRequestDto)
    {
        return Ok(await _eventService.GetFilteredAndPaginatedEvents(searchEventsRequestDto, new PaginationDto(0, 10)));
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetEventDetailsById(Guid id)
    {
        var dto = await _eventService.GetEventDetailsByEventId(id);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPut("{eventId:guid}/participate")]
    public async Task<IActionResult> SignUpForEvent(Guid eventId)
        => Ok(await _eventService.SignUpForEvent(eventId));
    
    [HttpPut("{eventId:guid}/withdraw")]
    public async Task<IActionResult> WithdrawFromEvent(Guid eventId)
        => Ok(await _eventService.WithdrawFromEvent(eventId));
    
    [HttpPost("{eventId:guid}/create-payment-intent")]
    public async Task<IActionResult> CreatePaymentIntent([FromBody] CreatePaymentRequestDto request)
    {
        var metadata = new Dictionary<string, string>
        {
            { "eventId", request.EventId.ToString() },
            { "userId", request.UserId.ToString() }
        };

        var paymentIntent = await _stripeService.CreatePaymentIntent(request.Amount, metadata);
        return Ok(new { clientSecret = paymentIntent.ClientSecret });
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CreateEventRequestDto req)
    {
        var created = await _eventService.AddEvent(req);
        return CreatedAtAction(nameof(Create), new { id = created!.EventId }, created);
    }

    [HttpPost("{eventId:guid}/review")]
    public async Task<IActionResult> CreateReview(Guid eventId, [FromBody] CreateEventReviewRequestDto req)
    {
        var reviewId = await _eventService.CreateReview(eventId, req);
        return CreatedAtAction(nameof(CreateReview), new { eventId, reviewId }, new { reviewId });
    }

    [AllowAnonymous]
    [HttpGet("{eventId:guid}/reviews")]
    public async Task<IActionResult> GetAllReviewsForEvent(Guid eventId)
    {
        var reviews = await _eventService.GetAllReviewsForAnEvent(eventId);
        return Ok(reviews);
    }

    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetByUser(Guid userId)
        => Ok(await _eventService.GetEventsByUserId(userId));

    [HttpGet("user-as-participant/{userId:guid}")]
    public async Task<IActionResult> GetByUserParticipant(Guid userId)
        => Ok(await _eventService.GetEventsWhereUserIsParticipant(userId));

    [HttpGet("user-reviews-as-host/{userId:guid}")]
    public async Task<IActionResult> GetEventReviewsByUserId(Guid userId)
        => Ok(await _eventService.GetEventReviewsByUserId(userId));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _eventService.DeleteEvent(id);
        return deleted is null ? NotFound() : Ok(deleted);
    }

    [AllowAnonymous]
    [HttpPost("stripe/webhhok")]
    public async Task<IActionResult> StripeWebhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        var endpointSecret = Environment.GetEnvironmentVariable("STRIPE_WEBHOOK_SECRET");

        try
        {
            var stripeEvent = EventUtility.ConstructEvent(
                json,
                Request.Headers["Stripe-Signature"],
                endpointSecret
            );

            if (stripeEvent.Type == "payment_intent.succeeded")
            {
                var paymentIntent = stripeEvent.Data.Object as Stripe.PaymentIntent;
                var paymentIntentId = paymentIntent.Id;

                await _eventService.HandlePaymentSuccess(paymentIntentId);
            }

            return Ok();
        }
        catch (StripeException e)
        {
            return BadRequest(e.Message);
        }
    }
}
