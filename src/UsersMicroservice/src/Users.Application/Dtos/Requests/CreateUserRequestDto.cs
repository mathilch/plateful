namespace Users.Application.Dtos.Requests;

public class CreateUserRequestDto
{
    public string Name { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string Password { get; set; } = default!;
    public int Age { get; init; }
}
