﻿using Microsoft.Extensions.Caching.Memory;
using Moq;
using UserApi.Application.DTOs;
using UserApi.Application.Features.User.Commands;
using UserApi.Application.Handlers;
using UserApi.Application.Services;
using Xunit;

namespace UserApi.Test
{
    public class CreateUserCommandHandlerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly CreateUserCommandHandler _handler;
        private readonly Mock<IMemoryCache> _mockCache;
        public CreateUserCommandHandlerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _mockCache = new Mock<IMemoryCache>();
            _handler = new CreateUserCommandHandler(_mockUserService!.Object, _mockCache.Object);            
        }

        [Fact]
        public async Task Handle_ShouldCreateUserAndReturnId()
        {
            var userDto = new UserDto { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
            var command = new CreateUserCommand { User = userDto };

            var userId = 1;
            _mockUserService.Setup(x => x.AddUserAsync(userDto).Result);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.Equal(userId, result);
        }
    }
}
