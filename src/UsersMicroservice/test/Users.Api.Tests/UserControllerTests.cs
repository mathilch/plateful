using System;
using Microsoft.AspNetCore.Mvc;
using Users.Api.Controllers;
using Users.Application.Dtos;
using Users.Application.Dtos.Requests;
using Xunit;

namespace Users.Api.Tests
{
    public class UserControllerTests
    {
        private readonly InMemoryUserService _userService;
        private readonly UserController _userController;

        public UserControllerTests()
        {
            _userService = new InMemoryUserService();
            _userController = new UserController(_userService);
        }
        
        [Fact]
        public async Task GetUserById_ReturnsUser()
        {
            var userId = await _userService.CreateUserAsync(new CreateUserRequestDto
            {
                Name = "Mathias",
                Email = "mathias@plateful.dk",
                Password = "onedirection",
                Birthday = new DateOnly(1990, 1, 1)
            });

            var result = await _userController.GetUserById(userId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var user = Assert.IsType<UserDto>(okResult.Value);
            Assert.Equal("Mathias", user.Name);
        }
    }
}