using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickServeAPP.DTOs;
using QuickServeAPP.Services;

namespace QuickServeAPP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitRating([FromBody] SubmitRatingDto ratingDto)
        {
            try
            {
                await _ratingService.SubmitRatingAsync(ratingDto);
                return Ok(new { Message = "Rating submitted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("restaurant/{restaurantId}")]
        public async Task<IActionResult> GetRatingsByRestaurantId(int restaurantId)
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

        [HttpGet("restaurant/{restaurantId}/average")]
        public async Task<IActionResult> GetAverageRatingByRestaurantId(int restaurantId)
        {
            try
            {
                var averageRating = await _ratingService.GetAverageRatingByRestaurantIdAsync(restaurantId);
                return Ok(new { AverageRating = averageRating });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("menu/{menuId}")]
        public async Task<IActionResult> GetRatingsByMenuId(int menuId)
        {
            try
            {
                var ratings = await _ratingService.GetRatingsByMenuIdAsync(menuId);
                return Ok(ratings);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("menu/{menuId}/average")]
        public async Task<IActionResult> GetAverageRatingByMenuId(int menuId)
        {
            try
            {
                var averageRating = await _ratingService.GetAverageRatingByMenuIdAsync(menuId);
                return Ok(new { AverageRating = averageRating });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }

}
