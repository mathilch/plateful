using System;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Users.Application.Contracts.Repositories;
using Users.Application.Dtos.Requests;
using Users.Application.Services;
using Users.Application.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Users.Domain.Entities;
using Xunit;

namespace Users.Application.Tests;
  

public sealed class UserServiceTests
{
  private readonly InMemoryUserRepository _userRepo;
  private readonly UserService _userService;

  public UserServiceTests()
  {
    _userRepo = new InMemoryUserRepository();
    
    var services = new ServiceCollection();
    services.AddSingleton<IValidator<CreateUserRequestDto>, CreateUserRequestValidator>();
    var serviceProvider = services.BuildServiceProvider();
    var httpContext = new DefaultHttpContext
    {
      RequestServices = serviceProvider
    };

    var httpContextAccessor = new HttpContextAccessor
    {
      HttpContext = httpContext
    };
    
    _userService = new UserService(_userRepo, new DummyTokenService(), httpContextAccessor);
  }

  
  [Fact]
  public async Task CreateUser_ShouldStoreUser()
  {
    var createDto = new CreateUserRequestDto
    {
      Name = "Mathias",
      Email = "mathias@plateful.dk",
      Password = "onedirection",
      Birthday = new DateOnly(1990, 1, 1)
    };

    var userId = await _userService.CreateUserAsync(createDto);
    var user = await _userRepo.GetUserById(userId);

    Assert.NotNull(user);
    Assert.Equal("Mathias", user.Name);
    Assert.Equal("mathias@plateful.dk", user.Email);
    Assert.True(user.IsActive);
  }
}
