namespace Users.Application.Dtos;

public record UserDto(
    Guid Id, 
    string Name, 
    string Email,
    bool Verified,
    float Score,
    DateOnly Birthday, 
    DateTime CreatedDate, 
    bool IsActive);
