using Microsoft.AspNetCore.Mvc;
using QuickServeAPP.Services;
using QuickServeAPP.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickServeAPP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpPost]
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

        [HttpGet]
        public async Task<IActionResult> GetAllRestaurants()
        {
            try
            {
                var restaurants = await _restaurantService.GetAllRestaurantsAsync();
                return Ok(restaurants);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
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

        [HttpPut("{id}")]
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRestaurant(int id)
        {
            try
            {
                await _restaurantService.DeleteRestaurantAsync(id);
                return Ok(new { Message = "Restaurant deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{id}/exists")]
        public async Task<IActionResult> CheckRestaurantExists(int id)
        {
            var exists = _restaurantService.DoesRestaurantExist(id);
            if (exists)
                return Ok(new { exists = true });
            else
                return NotFound(new { exists = false });
        }


        [HttpGet("search-by-item")]
        public IActionResult SearchRestaurantsByItem(string itemName)
        {
            if (string.IsNullOrEmpty(itemName))
                return BadRequest("Item name is required");

            var restaurants = _restaurantService.GetRestaurantsByItem(itemName);
            return Ok(restaurants);
        }

        [HttpGet("getTrendingRestaurants")]
        public IActionResult GetTrendingRestaurants()
        {
            try
            {
                var trendingRestaurants = _restaurantService.GetTrendingRestaurants();

                // Check if restaurants are available
                if (!trendingRestaurants.Any())
                {
                    return NotFound("No trending restaurants available.");
                }

                return Ok(trendingRestaurants);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching trending restaurants.");
            }
        }


    }
}


