using NUnit.Framework;
using Moq;
using QuickServeAPP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using QuickServeAPP.Controllers;
using QuickServeAPP.DTOs;
using QuickServeAPP.Services;

namespace Controllers.Tests
{
    [TestFixture]
    public class UserControllerTests
    {
        private Mock<IUserService> _mockUserService;
        private UserController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockUserService = new Mock<IUserService>();
            _controller = new UserController(_mockUserService.Object);
        }

        [Test]
        public async Task Register_ShouldReturnOk_WhenServiceSucceeds()
        {
            var registerDto = new RegisterDto { Email = "user1@example.com", Password = "password123" };
            _mockUserService.Setup(u => u.RegisterUserAsync(registerDto)).ReturnsAsync(new User());

            var result = await _controller.Register(registerDto);

            Assert.IsInstanceOf<CreatedAtActionResult>(result);
            var createdResult = result as CreatedAtActionResult;
            Assert.NotNull(createdResult);
            _mockUserService.Verify(u => u.RegisterUserAsync(registerDto), Times.Once);
        }

        [Test]
        public async Task GetUserById_ShouldReturnOk_WithUserDetails()
        {
            int userId = 1;
            var mockUser = new User { UserID = userId, Name = "user1", Email = "user1@example.com" };

            _mockUserService.Setup(u => u.GetUserByIdAsync(userId)).ReturnsAsync(mockUser);

            var result = await _controller.GetUserById(userId);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(mockUser, okResult.Value);
        }

        [Test]
        public async Task UpdateUser_ShouldReturnOk_WhenServiceSucceeds()
        {
            int userId = 1;
            var updateUserDto = new UpdateUserDto
            {
                Name = "Updated User",
                ContactNumber = "1234567890",
                Password = "newPassword"
            };

            var updatedUser = new User
            {
                UserID = userId,
                Name = updateUserDto.Name,
                ContactNumber = updateUserDto.ContactNumber
            };

            _mockUserService.Setup(u => u.UpdateUserAsync(userId, updateUserDto))
                            .ReturnsAsync(updatedUser);

            var result = await _controller.UpdateUser(userId, updateUserDto);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(updatedUser, okResult.Value);
            _mockUserService.Verify(u => u.UpdateUserAsync(userId, updateUserDto), Times.Once);
        }
    }
}
