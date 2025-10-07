using Users.Application.Dtos;
using Users.Domain.Entities;

namespace Users.Application.Mappers;

public static class UserMapper
{
    public static UserDto? ToDto(this User? user)
    {
        if (user is null) return null;
        return new UserDto(user.Id, user.Name, user.Email, user.CreatedDate, user.IsActive);
    }
}
