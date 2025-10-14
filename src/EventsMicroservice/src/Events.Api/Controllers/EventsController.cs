using Events.Application.Contracts.Repositories;
using Events.Application.Dtos.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Events.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventRepository _repo;
    public EventsController(IEventRepository repo) => _repo = repo;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _repo.GetAllEvents());

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var dto = await _repo.GetEventById(id);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateEventRequestDto req)
    {
        var created = await _repo.AddEvent(req);
        return CreatedAtAction(nameof(GetById), new { id = created!.EventId }, created);
    }

    [HttpGet("user/{userId:guid}")]
    public async Task<IActionResult> GetByUser(Guid userId)
        => Ok(await _repo.GetEventByUserId(userId));

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _repo.DeleteEvent(id);
        return deleted is null ? NotFound() : Ok(deleted);
    }
}
