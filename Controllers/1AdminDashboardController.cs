using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickServeAPP.Services;
using QuickServeAPP.DTOs;

namespace QuickServeAPP.Controllers
{
    //[Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/admin/dashboard")]
    public class AdminDashboardController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRestaurantService _restaurantService;
        private readonly IOrderService _orderService;
        private readonly IReportService _reportService;
        private readonly IRatingService _ratingService;

        public AdminDashboardController(IUserService userService, IRestaurantService restaurantService, IOrderService orderService, IReportService reportService, IRatingService ratingService)
        {
            _userService = userService;
            _restaurantService = restaurantService;
            _orderService = orderService;
            _reportService = reportService;
            _ratingService = ratingService;
        }

        [HttpGet("reports")]
        public async Task<IActionResult> GetSystemReport()
        {
            var report = await _reportService.GenerateSystemReportAsync();
            return Ok(report);
        }

        // Get all users
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // Get all restaurants
        [HttpGet("restaurants")]
        public async Task<IActionResult> GetAllRestaurants()
        {
            var restaurants = await _restaurantService.GetAllRestaurantsAsync();
            return Ok(restaurants);
        }


        // Get all orders
        [HttpGet("orders")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        // Remove a user
        [HttpDelete("user/{id}")]
        public async Task<IActionResult> RemoveUser(int id)
        {
            var result = await _userService.RemoveUserAsync(id);
            if (!result)
                return NotFound(new { Message = "User not found or could not be removed." });

            return Ok(new { Message = "User successfully removed." });
        }

        // Remove a restaurant
        [HttpDelete("restaurant/{id}")]
        public async Task<IActionResult> RemoveRestaurant(int id)
        {
            var result = await _restaurantService.DeleteRestaurantAsync(id);
            if (!result)
                return NotFound(new { Message = "Restaurant not found or could not be removed." });

            return Ok(new { Message = "Restaurant successfully removed." });
        }

        // Approve or suspend a restaurant
        [HttpPut("restaurant/status/{id}")]
        public async Task<IActionResult> UpdateRestaurantStatus(int id, [FromBody] UpdateRestaurantStatusDto statusDto)
        {
            var result = await _restaurantService.UpdateRestaurantStatusAsync(id, statusDto.IsActive);
            if (!result)
                return NotFound(new { Message = "Restaurant not found or could not be updated." });

            return Ok(new { Message = $"Restaurant successfully {(statusDto.IsActive ? "approved" : "suspended")}." });
        }

        [HttpPut("restaurant/update/{id}")]
        public async Task<IActionResult> UpdateRestaurant(int id, [FromBody] RestaurantDto restaurantDto)
        {
            try
            {
                var updatedRestaurant = await _restaurantService.UpdateRestaurantAsync(id, restaurantDto);
                return Ok(updatedRestaurant);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("Restaurant")]
        public async Task<IActionResult> AddRestaurant([FromBody] RestaurantDto restaurantDto)
        {
            try
            {
                var newRestaurant = await _restaurantService.CreateRestaurantAsync(restaurantDto);

                return CreatedAtAction(nameof(GetRestaurantById), new { id = newRestaurant.RestaurantID }, newRestaurant);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("Restaurant/{id}")]
        public async Task<IActionResult> GetRestaurantById(int id)
        {
            try
            {
                var restaurant = await _restaurantService.GetRestaurantByIdAsync(id);
                return Ok(restaurant);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("orders/filter")]
        public async Task<IActionResult> FilterOrders([FromQuery] string status)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(status))
                {
                    return BadRequest(new { message = "Order status is required." });
                }

                var orders = await _orderService.GetOrdersByStatusAsync(status);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("ratings")]
        public async Task<IActionResult> GetAllRatings()
        {
            try
            {
                var ratings = await _ratingService.GetAllRatingsAsync();
                return Ok(ratings);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


    }
}
