using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersfluentTestingAPI.API.Controllers;
using UsersfluentTestingAPI.API.Models;

namespace UsersfluentxUnitTesting.xunitTesting
{
    public class UsersControllerTests
    {
        private readonly UsersController _controller;
        private readonly Mock<IUserService> _mockService;
        public UsersControllerTests()
        {
            // Create a mock IUserService
            _mockService = new Mock<IUserService>();
            // Inject the mock service into the controller
            _controller = new UsersController(_mockService.Object);
        }
        [Fact]
        public async Task GetAllUsersAsync_ReturnsOkResultWithListOfUsers()
        {
            // Arrange
            var expectedUsers = new List<User>
            {
                new User { Id = 1, Name = "John Doe", Email = "john@example.com" },
                new User { Id = 2, Name = "Jane Smith", Email = "jane@example.com" }
            };
            _mockService.Setup(service => service.GetAllUsersAsync()).ReturnsAsync(expectedUsers);
            // Act
            var result = await _controller.GetAllUsersAsync() as OkObjectResult;
            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(expectedUsers, result.Value);
        }
        [Theory]
        [ClassData(typeof(GetUserByIdTestData))]
        public async Task GetUserByIdAsync_ReturnsExpectedResult(int userId, bool userExists)
        {
            // Arrange
            var expectedUser = userExists ? new User { Id = userId, Name = "Test User", Email = "test@example.com" } : null;
            _mockService.Setup(service => service.GetUserByIdAsync(userId)).ReturnsAsync(expectedUser);
            // Act
            var result = await _controller.GetUserByIdAsync(userId);
            // Assert
            if (userExists)
            {
                var okResult = Assert.IsType<OkObjectResult>(result);
                Assert.NotNull(okResult.Value);
                Assert.Equal(userId, ((User)okResult.Value).Id);
            }
            else
            {
                Assert.IsType<NotFoundResult>(result);
            }
        }
        [Fact]
        public async Task AddUserAsync_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var newUser = new User { Id = 3, Name = "Sam Wilson", Email = "sam@example.com" };
            _mockService.Setup(service => service.AddUserAsync(newUser)).Returns(Task.CompletedTask);
            // Act
            var result = await _controller.AddUserAsync(newUser) as CreatedAtActionResult;
            // Assert
            Assert.NotNull(result);
            Assert.Equal(201, result.StatusCode);
            Assert.Equal("GetUserByIdAsync", result.ActionName);
            Assert.Equal(newUser.Id, ((User)result.Value).Id);
        }
        [Fact]
        public async Task UpdateUserAsync_ReturnsNoContent()
        {
            // Arrange
            var updatedUser = new User { Id = 1, Name = "John Updated", Email = "john.updated@example.com" };
            _mockService.Setup(service => service.UpdateUserAsync(updatedUser)).Returns(Task.CompletedTask);
            // Act
            var result = await _controller.UpdateUserAsync(1, updatedUser) as NoContentResult;
            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }
        [Fact]
        public async Task UpdateUserAsync_ReturnsBadRequest_WhenIdMismatch()
        {
            // Arrange
            var updatedUser = new User { Id = 2, Name = "John Updated", Email = "john.updated@example.com" };
            // Act
            var result = await _controller.UpdateUserAsync(1, updatedUser) as BadRequestResult;
            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }
        [Fact]
        public async Task DeleteUserAsync_ReturnsNoContent()
        {
            // Arrange
            var userId = 1;
            _mockService.Setup(service => service.DeleteUserAsync(userId)).Returns(Task.CompletedTask);
            // Act
            var result = await _controller.DeleteUserAsync(userId) as NoContentResult;
            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }
    }
}
