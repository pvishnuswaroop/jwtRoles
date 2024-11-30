using Moq;
using NUnit.Framework;
using QuickServeAPP.Controllers;
using QuickServeAPP.DTOs;
using QuickServeAPP.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Controllers.Tests
{
    [TestFixture]
    public class CartControllerTests
    {
        private Mock<ICartService> _mockCartService;
        private CartController _controller;

        [SetUp]
        public void Setup()
        {
            // Create mock instance of ICartService
            _mockCartService = new Mock<ICartService>();
            // Initialize the controller with the mocked service
            _controller = new CartController(_mockCartService.Object);
        }

        // Test for GetCart method
        [Test]
        public async Task GetCart_ShouldReturnOk_WhenCartExists()
        {
            // Arrange
            int userId = 1;
            var mockCart = new CartDto
            {
                CartID = 1,
                UserID = userId,
                IsActive = true,
                CreationDate = DateTime.UtcNow
            };

            _mockCartService.Setup(service => service.GetOrCreateCartByUserIdAsync(userId))
                            .ReturnsAsync(mockCart);

            // Act
            var result = await _controller.GetCart(userId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnValue = okResult.Value as CartDto;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual(userId, returnValue.UserID);
        }

        // Test for AddItemToCart method
        [Test]
        public async Task AddItemToCart_ShouldReturnOk_WhenItemAdded()
        {
            // Arrange
            int userId = 1;
            var cartItem = new CartItemDto
            {
                MenuID = 1,
                Quantity = 2,
                Price = 10.5m
            };
            var updatedCart = new CartDto
            {
                CartID = 1,
                UserID = userId,
                IsActive = true,
                CreationDate = DateTime.UtcNow
            };

            _mockCartService.Setup(service => service.AddItemToCartAsync(userId, cartItem))
                            .ReturnsAsync(updatedCart);

            // Act
            var result = await _controller.AddItemToCart(userId, cartItem);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnValue = okResult.Value as CartDto;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual(userId, returnValue.UserID);
        }

        // Test for RemoveItemFromCart method
        [Test]
        public async Task RemoveItemFromCart_ShouldReturnOk_WhenItemRemoved()
        {
            // Arrange
            int userId = 1;
            int menuId = 1;
            var updatedCart = new CartDto
            {
                CartID = 1,
                UserID = userId,
                IsActive = true,
                CreationDate = DateTime.UtcNow
            };

            _mockCartService.Setup(service => service.RemoveItemFromCartAsync(userId, menuId))
                            .ReturnsAsync(updatedCart);

            // Act
            var result = await _controller.RemoveItemFromCart(userId, menuId);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnValue = okResult.Value as CartDto;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual(userId, returnValue.UserID);
        }

        // Test for UpdateItemQuantity method
        [Test]
        public async Task UpdateItemQuantity_ShouldReturnOk_WhenQuantityUpdated()
        {
            // Arrange
            int userId = 1;
            int menuId = 1;
            int newQuantity = 3;
            var updatedCart = new CartDto
            {
                CartID = 1,
                UserID = userId,
                IsActive = true,
                CreationDate = DateTime.UtcNow
            };

            _mockCartService.Setup(service => service.UpdateItemQuantityAsync(userId, menuId, newQuantity))
                            .ReturnsAsync(updatedCart);

            // Act
            var result = await _controller.UpdateItemQuantity(userId, menuId, newQuantity);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            var returnValue = okResult.Value as CartDto;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual(userId, returnValue.UserID);
        }
    }
}
