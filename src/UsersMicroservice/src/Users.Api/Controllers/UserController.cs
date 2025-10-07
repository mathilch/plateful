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
}
