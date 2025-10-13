namespace Users.Application.Dtos;

public record UserDto(Guid Id, string Name, string Email, DateTime CreatedDate, bool IsActive);
