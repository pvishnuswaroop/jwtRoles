using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickServeAPP.DTOs;
using QuickServeAPP.Services;

namespace QuickServeAPP.Controllers
{
    // CartController.cs
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetCart(int userId)
        {
            try
            {
                var cart = await _cartService.GetOrCreateCartByUserIdAsync(userId);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("user/{userId}")]
        public async Task<IActionResult> AddItemToCart(int userId, [FromBody] CartItemDto cartItemDto)
        {
            try
            {
                var updatedCart = await _cartService.AddItemToCartAsync(userId, cartItemDto);
                return Ok(updatedCart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("user/{userId}/item/{menuId}")]
        public async Task<IActionResult> RemoveItemFromCart(int userId, int menuId)
        {
            try
            {
                var updatedCart = await _cartService.RemoveItemFromCartAsync(userId, menuId);
                return Ok(updatedCart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("user/{userId}/item/{menuId}")]
        public async Task<IActionResult> UpdateItemQuantity(int userId, int menuId, [FromBody] int quantity)
        {
            try
            {
                var updatedCart = await _cartService.UpdateItemQuantityAsync(userId, menuId, quantity);
                return Ok(updatedCart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("user/{userId}/clear")]
        public async Task<IActionResult> ClearCart(int userId)
        {
            try
            {
                await _cartService.ClearCartAsync(userId);
                return Ok(new { Message = "Cart cleared successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }

}
