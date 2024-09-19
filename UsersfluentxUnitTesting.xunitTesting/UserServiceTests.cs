using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersfluentTestingAPI.API.Models;

namespace UsersfluentxUnitTesting.xunitTesting
{
    public class UserServiceTests
    {
        // Private fields to hold instances of UserService and the mock repository
        private readonly UserService _userService;
        private readonly Mock<IUserRepository> _mockRepository;
        // Constructor to initialize the mock repository and the service under test
        public UserServiceTests()
        {
            // Creating a mock instance of IUserRepository
            _mockRepository = new Mock<IUserRepository>();
            // Initializing the UserService with the mock repository
            _userService = new UserService(_mockRepository.Object);
        }
        // Theory attribute to indicate parameterized tests using MemberData
        [Theory]
        // MemberData attribute specifies the static property providing test data
        [MemberData(nameof(GetUserByIdData))]
        public async Task GetUserById_ReturnsExpectedResult(int userId, User expectedUser)
        {
            // Arrange
            // Setting up the mock repository to return the expected user when GetUserByIdAsync is called with the specified userId
            // This means when call _userService.GetUserByIdAsync(userId) method, the GetUserByIdAsync() method will not invoked the actual
            // _userRepository.GetUserByIdAsync(userId) method instead of, it will use the following mock repo.GetUserByIdAsync(userId) and 
            // Whatever data we passed in expectedUser, that will be return
            _mockRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ReturnsAsync(expectedUser);
            // Act
            // Calling the GetUserByIdAsync method of the service under test
            var result = await _userService.GetUserByIdAsync(userId);
            // Assert
            if (expectedUser == null)
            {
                // Providing a user-friendly error message if the result is not null when expected to be null
                result.Should().BeNull("because the user with ID {0} does not exist", userId);
            }
            else
            {
                // Providing user-friendly error messages for the assertions
                result.Should().NotBeNull("because the user with ID {0} exists", userId);
                result.Should().BeEquivalentTo(expectedUser, "because the returned user should match the expected user");
            }
        }
        // Fact attribute indicates a test method with no parameters
        [Fact]
        public async Task GetAllUsers_ReturnsListOfUsers()
        {
            // Arrange
            // Creating a list of expected users
            var expectedUsers = new List<User>
            {
                new User { Id = 1, Name = "John Doe", Email = "john@example.com" },
                new User { Id = 2, Name = "Jane Smith", Email = "jane@example.com" },
                new User { Id = 3, Name = "Pam Sara", Email = "pam@example.com" }
            };
            // Setting up the mock repository to return the expected users list when GetAllUsersAsync is called
            // This means when call _userService.GetAllUsersAsync(), the GetAllUsersAsync() method will not invoked the actual
            // _userRepository.GetAllUsersAsync() method instead of it will use the following mock GetAllUsersAsync() and 
            // Whatever data we passed in expectedUsers collection, that will be return
            _mockRepository.Setup(repo => repo.GetAllUsersAsync()).ReturnsAsync(expectedUsers);
            // Act
            // Calling the GetAllUsersAsync method of the service under test
            var result = await _userService.GetAllUsersAsync();
            // Assert
            // Providing user-friendly error messages for the assertions
            result.Should().NotBeNull("because the repository should return a list of users");
            result.Should().HaveCount(expectedUsers.Count, "because the number of users returned should match the expected count");
            result.Should().BeEquivalentTo(expectedUsers, "because the returned list of users should match the expected list");
        }
        [Fact]
        public async Task AddUser_CallsRepository()
        {
            // Arrange
            // Creating a new user to be added
            var newUser = new User { Id = 3, Name = "Sam Wilson", Email = "sam@example.com" };
            // Act
            // Calling the AddUserAsync method of the service under test
            await _userService.AddUserAsync(newUser);
            // Assert
            // Verifying that the AddUserAsync method of the mock repository was called exactly once with the new user
            _mockRepository.Verify(repo => repo.AddUserAsync(newUser), Times.Once, "because the AddUserAsync method should be called once to add the new user");
        }
        [Fact]
        public async Task UpdateUser_CallsRepository()
        {
            // Arrange
            // Creating an updated user
            var updatedUser = new User { Id = 1, Name = "John Updated", Email = "john.updated@example.com" };
            // Act
            // Calling the UpdateUserAsync method of the service under test
            await _userService.UpdateUserAsync(updatedUser);
            // Assert
            // Verifying that the UpdateUserAsync method of the mock repository was called exactly once with the updated user
            _mockRepository.Verify(repo => repo.UpdateUserAsync(updatedUser), Times.Once, "because the UpdateUserAsync method should be called once to update the user details");
        }
        [Fact]
        public async Task DeleteUser_CallsRepository()
        {
            // Arrange
            // Specifying the userId to be deleted
            var userId = 1;
            // Act
            // Calling the DeleteUserAsync method of the service under test
            await _userService.DeleteUserAsync(userId);
            // Assert
            // Verifying that the DeleteUserAsync method of the mock repository was called exactly once with the specified userId
            _mockRepository.Verify(repo => repo.DeleteUserAsync(userId), Times.Once, "because the DeleteUserAsync method should be called once to delete the user");
        }
        [Fact]
        public async Task GetUserById_ThrowsExceptionWhenRepositoryFails()
        {
            // Arrange
            // Specifying the userId that will cause the repository to throw an exception
            var userId = 1;
            // Setting up the mock repository to throw an exception when GetUserByIdAsync is called with the specified userId
            _mockRepository.Setup(repo => repo.GetUserByIdAsync(userId)).ThrowsAsync(new Exception("Repository failed"));
            // Act
            // Defining a function to call the GetUserByIdAsync method of the service under test
            Func<Task> act = async () => await _userService.GetUserByIdAsync(userId);
            // Assert
            // Asserting that calling the function throws an exception with the specified message
            await act.Should().ThrowAsync<Exception>().WithMessage("Repository failed", "because the repository is set up to fail when getting the user by ID");
        }
        // Static property providing test data for GetUserByIdAsync test cases
        public static IEnumerable<object[]> GetUserByIdData =>
            new List<object[]>
            {
                // Test case: userId 1 should return a User object
                new object[] { 1, new User { Id = 1, Name = "John Doe", Email = "john@example.com" } },
                // Test case: userId 99 should return null
                new object[] { 99, null }
            };
    }
}
