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
public class EventController(IEventService _eventService) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("recent")]
    public async Task<IActionResult> GetRecentEvents()
    {
        return Ok(await _eventService.GetRecentEvents(new PaginationDto(0, 10)));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _eventService.GetAllEvents());

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var dto = await _eventService.GetEventByEventId(id);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateEventRequestDto req)
    {
        var created = await _eventService.AddEvent(req);
        return CreatedAtAction(nameof(GetById), new { id = created!.EventId }, created);
    }

    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetByUser(Guid userId)
        => Ok(await _eventService.GetEventsByUserId(userId));

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
