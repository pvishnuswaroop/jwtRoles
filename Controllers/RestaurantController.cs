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
                var isDeleted = await _restaurantService.DeleteRestaurantAsync(id);
                if (!isDeleted)
                {
                    return NotFound(new { Message = "Restaurant not found." });
                }

                return NoContent(); // HTTP 204 for successful deletion
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


    }
}


