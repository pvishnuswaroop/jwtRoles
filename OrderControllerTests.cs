using NUnit.Framework;
using Moq;
using QuickServeAPP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using QuickServeAPP.Controllers;
using QuickServeAPP.DTOs;
using QuickServeAPP.Services;
using System.Collections.Generic;

namespace Controllers.Tests
{
    [TestFixture]
    public class OrderControllerTests
    {
        private Mock<IOrderService> _mockOrderService;
        private OrderController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockOrderService = new Mock<IOrderService>();
            _controller = new OrderController(_mockOrderService.Object);
        }

        [Test]
        public async Task PlaceOrder_ShouldReturnCreatedAtAction_WhenServiceSucceeds()
        {
            // Arrange
            var orderDto = new OrderDto
            {
                UserID = 1,
                RestaurantID = 1,
                TotalAmount = 50.0m,
                Address = "Test Address",
                OrderItems = new List<OrderItemDto>
        {
            new OrderItemDto { MenuID = 1, Quantity = 2 }
        }
            };
            var newOrder = new OrderDto { OrderID = 1, UserID = 1, RestaurantID = 1, TotalAmount = 50.0m, Address = "Test Address" };
            _mockOrderService.Setup(o => o.PlaceOrderAsync(orderDto)).ReturnsAsync(newOrder);

            // Act
            var result = await _controller.PlaceOrder(orderDto);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result);  
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.NotNull(createdAtActionResult);
            Assert.AreEqual("GetOrderById", createdAtActionResult.ActionName);
            Assert.AreEqual(newOrder.OrderID, createdAtActionResult.RouteValues["id"]);
            Assert.AreEqual(newOrder, createdAtActionResult.Value);
            _mockOrderService.Verify(o => o.PlaceOrderAsync(orderDto), Times.Once);
        }


        [Test]
        public async Task GetOrderById_ShouldReturnOk_WithOrder()
        {
            // Arrange
            int id = 1;
            var mockOrder = new OrderDto
            {
                OrderID = 1,
                UserID = 1,
                RestaurantID = 1,
                Address = "Test Address",
                TotalAmount = 50.0m,
                OrderStatus = "Pending",
                OrderItems = new List<OrderItemDto>
                {
                    new OrderItemDto { MenuID = 1, Quantity = 2 }
                }
            };
            _mockOrderService.Setup(o => o.GetOrderByIdAsync(id)).ReturnsAsync(mockOrder);

            // Act
            var result = await _controller.GetOrderById(id);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(mockOrder, okResult.Value);
        }

        [Test]
        public async Task GetOrdersByUserId_ShouldReturnOk_WithOrders()
        {
            // Arrange
            int userId = 1;
            var mockOrders = new List<OrderDto>
            {
                new OrderDto { OrderID = 1, UserID = 1, RestaurantID = 1, Address = "Test Address", TotalAmount = 30.0m },
                new OrderDto { OrderID = 2, UserID = 1, RestaurantID = 2, Address = "Test Address", TotalAmount = 70.0m }
            };
            _mockOrderService.Setup(o => o.GetOrdersByUserIdAsync(userId)).ReturnsAsync(mockOrders);

            // Act
            var result = await _controller.GetOrdersByUserId(userId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(mockOrders, okResult.Value);
        }

        [Test]
        public async Task GetOrdersByRestaurantId_ShouldReturnOk_WithOrders()
        {
            // Arrange
            int restaurantId = 1;
            var mockOrders = new List<OrderDto>
            {
                new OrderDto { OrderID = 1, UserID = 1, RestaurantID = 1, Address = "Test Address", TotalAmount = 100.0m },
                new OrderDto { OrderID = 2, UserID = 1, RestaurantID = 1, Address = "Test Address", TotalAmount = 150.0m }
            };
            _mockOrderService.Setup(o => o.GetOrdersByRestaurantIdAsync(restaurantId)).ReturnsAsync(mockOrders);

            // Act
            var result = await _controller.GetOrdersByRestaurantId(restaurantId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(mockOrders, okResult.Value);
        }

        [Test]
        public async Task UpdateOrderStatus_ShouldReturnOk_WhenServiceSucceeds()
        {
            // Arrange
            int orderId = 1;
            string status = "Completed";
            var orderDto = new OrderDto
            {
                OrderID = orderId,
                OrderStatus = status
            };
            _mockOrderService.Setup(o => o.UpdateOrderStatusAsync(orderId, status)).ReturnsAsync(orderDto);

            // Act
            var result = await _controller.UpdateOrderStatus(orderId, status);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.AreEqual(orderDto, okResult.Value);
            _mockOrderService.Verify(o => o.UpdateOrderStatusAsync(orderId, status), Times.Once);
        }

       
    }
}
