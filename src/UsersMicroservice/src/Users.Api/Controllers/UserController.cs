using Microsoft.AspNetCore.Mvc;
using Users.Application.Contracts.Services;
using Users.Application.Dtos.Requests;

namespace Users.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService _userService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginUserRequestDto request)
    {
        var token = await _userService.AuthenticateAndGenerateUserTokenAsync(request.Email, request.Password);
        return Ok(token);
    }

    [HttpPost]
    public async Task<IActionResult> AddUser([FromBody] CreateUserRequestDto request)
    {
        var userId = await _userService.CreateUserAsync(request);
        return Ok(userId);
    }

    [HttpPatch("{id}/deactivate-user")]
    public async Task<IActionResult> DeactivateUser([FromRoute] Guid id)
    {
        await _userService.DeactivateUserAsync(id);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
    {
        await _userService.DeleteUserAsync(id);
        return NoContent();
    }
}
