using NUnit.Framework;
using Moq;
using QuickServeAPP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuickServeAPP.Controllers;
using QuickServeAPP.DTOs;
using QuickServeAPP.Services;

namespace Controllers.Tests
{
    [TestFixture]
    public class RestaurantControllerTests
    {
        private Mock<IRestaurantService> _mockRestaurantService;
        private RestaurantController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockRestaurantService = new Mock<IRestaurantService>();
            _controller = new RestaurantController(_mockRestaurantService.Object);
        }

        [Test]
        public async Task AddRestaurant_ShouldReturnCreatedAtAction_WhenServiceSucceeds()
        {
            // Arrange
            var restaurantDto = new RestaurantDto { Name = "Pizza Place", Location = "New York" };
            var createdRestaurantDto = new RestaurantDto { RestaurantID = 1, Name = "Pizza Place", Location = "New York" };
            _mockRestaurantService.Setup(r => r.CreateRestaurantAsync(restaurantDto)).ReturnsAsync(createdRestaurantDto);

            // Act
            var result = await _controller.AddRestaurant(restaurantDto);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result);
            var createdResult = result as CreatedAtActionResult;
            Assert.NotNull(createdResult);
            Assert.AreEqual("GetRestaurantById", createdResult.ActionName);
            Assert.AreEqual(createdRestaurantDto.RestaurantID, createdResult.RouteValues["id"]);
            Assert.AreEqual(createdRestaurantDto, createdResult.Value);
            _mockRestaurantService.Verify(r => r.CreateRestaurantAsync(restaurantDto), Times.Once);
        }

        [Test]
        public async Task GetAllRestaurants_ShouldReturnOk_WithRestaurants()
        {
            // Arrange
            var mockRestaurants = new List<RestaurantDto>
            {
                new RestaurantDto { RestaurantID = 1, Name = "Pizza Place", Location = "New York" },
                new RestaurantDto { RestaurantID = 2, Name = "Burger Shack", Location = "Chicago" }
            };
            _mockRestaurantService.Setup(r => r.GetAllRestaurantsAsync()).ReturnsAsync(mockRestaurants);

            // Act
            var result = await _controller.GetAllRestaurants();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(mockRestaurants, okResult.Value);
        }

        [Test]
        public async Task GetRestaurantById_ShouldReturnOk_WithRestaurant()
        {
            // Arrange
            int id = 1;
            var mockRestaurant = new RestaurantDto { RestaurantID = id, Name = "Pizza Place", Location = "New York" };
            _mockRestaurantService.Setup(r => r.GetRestaurantByIdAsync(id)).ReturnsAsync(mockRestaurant);

            // Act
            var result = await _controller.GetRestaurantById(id);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(mockRestaurant, okResult.Value);
        }

        [Test]
        public async Task UpdateRestaurant_ShouldReturnOk_WhenServiceSucceeds()
        {
            // Arrange
            int id = 1;
            var restaurantDto = new RestaurantDto { Name = "Updated Pizza Place", Location = "Los Angeles" };
            _mockRestaurantService.Setup(r => r.UpdateRestaurantAsync(id, restaurantDto)).ReturnsAsync(restaurantDto);

            // Act
            var result = await _controller.UpdateRestaurant(id, restaurantDto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(restaurantDto, okResult.Value);
            _mockRestaurantService.Verify(r => r.UpdateRestaurantAsync(id, restaurantDto), Times.Once);
        }

        [Test]
        public async Task DeleteRestaurant_ShouldReturnNoContent_WhenServiceSucceeds()
        {
            // Arrange
            int id = 1;
            _mockRestaurantService.Setup(r => r.DeleteRestaurantAsync(id)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteRestaurant(id);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
            _mockRestaurantService.Verify(r => r.DeleteRestaurantAsync(id), Times.Once);
        }

    }
}
