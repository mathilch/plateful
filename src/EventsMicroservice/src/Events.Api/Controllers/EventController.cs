using Events.Application.Contracts.Repositories;
using Events.Application.Contracts.Services;
using Events.Application.Dtos.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Events.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventController(IEventService _eventService) : ControllerBase
{

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
}
