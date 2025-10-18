using Microsoft.AspNetCore.Identity;
using Users.Application.Contracts.Repositories;
using Users.Application.Contracts.Services;
using Users.Application.Dtos;
using Users.Application.Dtos.Requests;
using Users.Application.Exceptions;

namespace Users.Application.Services;

public class UserService(IUserRepository _userRepository, ITokenService _tokenService) : IUserService
{
    private readonly PasswordHasher<object> _passwordHasher = new();
    public async Task<Guid> CreateUserAsync(CreateUserRequestDto createUserDto)
    {
        var hashedPassword = _passwordHasher.HashPassword(createUserDto.Email, createUserDto.Password);
        createUserDto.Password = hashedPassword;

        var user = await _userRepository.AddUser(createUserDto);
        return user.Id;
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        await _userRepository.DeleteUser(userId);
    }

    public Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<string> AuthenticateAndGenerateUserTokenAsync(string email, string password)
    {
        var user = await _userRepository.GetUserByEmail(email);
        if (user.userDto is null)
        {
            throw new Exception("user not found");
        }
        var verificationResult = _passwordHasher.VerifyHashedPassword(user.userDto.Email, user.hashedPassword, password);
        if (verificationResult == PasswordVerificationResult.Success)
        {
            return _tokenService.CreateToken(user.userDto.Id, user.userDto.Email, user.userDto.Name);
        }

        throw new WrongUserCredentialsException(email);
    }

    public Task<UserDto?> GetUserByIdAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateUserAsync(Guid userId, string name, string email)
    {
        throw new NotImplementedException();
    }

    public async Task DeactivateUserAsync(Guid id)
    {
        await _userRepository.DeactivateUser(id);
    }
}
