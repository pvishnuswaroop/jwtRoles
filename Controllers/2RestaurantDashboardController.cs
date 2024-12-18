using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickServeAPP.DTOs;
using QuickServeAPP.Services;

namespace QuickServeAPP.Controllers
{
    //[Authorize(Roles = "RestaurantOwner")]
    [ApiController]
    [Route("api/restaurant/dashboard")]
    public class RestaurantDashboardController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMenuService _menuService;
        private readonly IRatingService _ratingService;

        public RestaurantDashboardController(IOrderService orderService, IMenuService menuService, IRatingService ratingService)
        {
            _orderService = orderService;
            _menuService = menuService;
            _ratingService = ratingService;
        }

        // Get all orders for the restaurant
        [HttpGet("orders")]
        public async Task<IActionResult> GetRestaurantOrders(int restaurantId)
        {
            
            try
            {
                var orders = await _orderService.GetOrdersByRestaurantIdAsync(restaurantId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // Update the status of an order
        [HttpPut("order/{orderId}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] string status)
        {
            var updatedOrder = await _orderService.UpdateOrderStatusAsync(orderId, status);

            if (updatedOrder == null)
                return NotFound(new { Message = "Order not found or could not be updated." });

            return Ok(new { Message = "Order status updated successfully.", UpdatedOrder = updatedOrder });
        }


        // Get menu for the restaurant
        [HttpGet("menu")]
        public async Task<IActionResult> GetMenu(int restaurantId)
        {
            
            try
            {
                var menuItems = await _menuService.GetMenusByRestaurantIdAsync(restaurantId);
                return Ok(menuItems);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // Add a menu item
        [HttpPost("menu")]
        public async Task<IActionResult> AddMenuItem([FromBody] MenuDto menuDto)
        {
            var addedMenuItem = await _menuService.CreateMenuAsync(menuDto);
            if (addedMenuItem == null)
                return BadRequest(new { Message = "Failed to add menu item." });

            return CreatedAtAction(nameof(AddMenuItem), new { id = addedMenuItem.MenuID }, addedMenuItem);
        }

        // Update a menu item
        [HttpPut("menu/{id}")]
        public async Task<IActionResult> UpdateMenuItem(int id, [FromBody] MenuDto menuDto)
        {
            menuDto.MenuID = id;
            var updatedMenuItem = await _menuService.UpdateMenuAsync(menuDto);
            if (updatedMenuItem == null)
                return NotFound(new { Message = "Menu item not found or could not be updated." });

            return Ok(updatedMenuItem);
        }

        // Delete a menu item
        [HttpDelete("menu/{id}")]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            var isDeleted = await _menuService.DeleteMenuAsync(id);
            if (!isDeleted)
                return NotFound(new { Message = "Menu item not found or could not be deleted." });

            return Ok(new { Message = "Menu item successfully deleted." });
        }


        // Get ratings for the restaurant
        [HttpGet("ratings")]
        public async Task<IActionResult> GetRestaurantRatings(int restaurantId)
        {
            try
            {
                var ratings = await _ratingService.GetRatingsByRestaurantIdAsync(restaurantId);
                return Ok(ratings);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
