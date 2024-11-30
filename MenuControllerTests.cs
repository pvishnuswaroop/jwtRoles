using Moq;
using NUnit.Framework;
using QuickServeAPP.Controllers;
using QuickServeAPP.Services;
using QuickServeAPP.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Controllers.Tests
{
    [TestFixture]
    public class MenuControllerTests
    {
        private Mock<IMenuService> _mockMenuService;
        private MenuController _controller;

        [SetUp]
        public void Setup()
        {
            _mockMenuService = new Mock<IMenuService>();
            _controller = new MenuController(_mockMenuService.Object);
        }

        [Test]
        public async Task AddMenuItem_ShouldReturnCreatedAtAction_WhenServiceSucceeds()
        {
            var menuDto = new MenuDto
            {
                RestaurantID = 1,
                ItemName = "Pizza",
                Description = "Delicious cheese pizza",
                Category = "Main Course",
                Price = 12.99m,
                AvailabilityTime = "10 AM - 10 PM",
                DietaryInfo = "Contains dairy",
                Status = "Available"
            };

            var createdMenu = new MenuDto
            {
                MenuID = 1,
                RestaurantID = 1,
                ItemName = "Pizza",
                Description = "Delicious cheese pizza",
                Category = "Main Course",
                Price = 12.99m,
                AvailabilityTime = "10 AM - 10 PM",
                DietaryInfo = "Contains dairy",
                Status = "Available"
            };

            _mockMenuService.Setup(service => service.CreateMenuAsync(menuDto)).ReturnsAsync(createdMenu);

            var result = await _controller.AddMenuItem(menuDto);

            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual("GetMenuItemById", createdAtActionResult.ActionName);
            Assert.AreEqual(createdMenu.MenuID, createdAtActionResult.RouteValues["id"]);
            Assert.AreEqual(createdMenu, createdAtActionResult.Value);
        }

        [Test]
        public async Task GetMenuByRestaurantId_ShouldReturnOk_WhenServiceSucceeds()
        {
            int restaurantId = 1;
            var menus = new List<MenuDto>
            {
                new MenuDto { MenuID = 1, RestaurantID = restaurantId, ItemName = "Pizza" },
                new MenuDto { MenuID = 2, RestaurantID = restaurantId, ItemName = "Burger" }
            };

            _mockMenuService.Setup(service => service.GetMenusByRestaurantIdAsync(restaurantId)).ReturnsAsync(menus);

            var result = await _controller.GetMenuByRestaurantId(restaurantId);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedMenus = okResult.Value as List<MenuDto>;
            Assert.AreEqual(2, returnedMenus.Count);
        }

        [Test]
        public async Task GetMenuItemById_ShouldReturnOk_WhenItemExists()
        {
            int menuId = 1;
            var menuItem = new MenuDto { MenuID = menuId, RestaurantID = 1, ItemName = "Pizza" };
            _mockMenuService.Setup(service => service.GetMenuByIdAsync(menuId)).ReturnsAsync(menuItem);

            var result = await _controller.GetMenuItemById(menuId);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(menuItem, okResult.Value);
        }

        [Test]
        public async Task UpdateMenuItem_ShouldReturnOk_WhenServiceSucceeds()
        {
            int menuId = 1;
            var menuDto = new MenuDto
            {
                MenuID = menuId,
                RestaurantID = 1,
                ItemName = "Pizza",
                Description = "Updated description",
                Category = "Main Course",
                Price = 15.99m,
                AvailabilityTime = "10 AM - 10 PM",
                DietaryInfo = "Contains dairy",
                Status = "Available"
            };

            var updatedMenu = new MenuDto
            {
                MenuID = menuId,
                RestaurantID = 1,
                ItemName = "Pizza",
                Description = "Updated description",
                Category = "Main Course",
                Price = 15.99m,
                AvailabilityTime = "10 AM - 10 PM",
                DietaryInfo = "Contains dairy",
                Status = "Available"
            };

            _mockMenuService.Setup(service => service.UpdateMenuAsync(menuDto)).ReturnsAsync(updatedMenu);

            var result = await _controller.UpdateMenuItem(menuId, menuDto);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(updatedMenu, okResult.Value);
        }

        [Test]
        public async Task AddMenuItem_ShouldReturnBadRequest_WhenModelIsInvalid()
        {
            var menuDto = new MenuDto
            {
                RestaurantID = 1,
                ItemName = "",
                Price = 12.99m,
                Status = "Available"
            };

            _controller.ModelState.AddModelError("ItemName", "Item name is required.");

            var result = await _controller.AddMenuItem(menuDto);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            var validationErrors = badRequestResult.Value as SerializableError;
            Assert.IsTrue(validationErrors.ContainsKey("ItemName"));
        }
    }
}
