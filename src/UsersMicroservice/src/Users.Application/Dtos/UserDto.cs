namespace Users.Application.Dtos;

public record UserDto(Guid Id, string Name, string Email, int Age, DateTime CreatedDate, bool IsActive);
