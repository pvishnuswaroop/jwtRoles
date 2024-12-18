using Microsoft.AspNetCore.Mvc;
using QuickServeAPP.Services;
using QuickServeAPP.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickServeAPP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        // POST: api/menu
        [HttpPost]
        public async Task<IActionResult> AddMenuItem([FromBody] MenuDto menuDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return validation errors
            }

            try
            {
                var newMenuItem = await _menuService.CreateMenuAsync(menuDto);
                return CreatedAtAction(nameof(GetMenuItemById), new { id = newMenuItem.MenuID }, newMenuItem);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        // GET: api/menu/restaurant/{restaurantId}
        [HttpGet("restaurant/{restaurantId}")]
        public async Task<IActionResult> GetMenuByRestaurantId(int restaurantId)
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

        [HttpGet("restaurant/{restaurantId}/sorted-menu")]
        public async Task<IActionResult> GetMenusWithRatingsByRestaurantId(int restaurantId)
        {
            try
            {
                var menus = await _menuService.GetMenusWithRatingsByRestaurantIdAsync(restaurantId);
                return Ok(menus);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        // GET: api/menu/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMenuItemById(int id)
        {
            try
            {
                var menuItem = await _menuService.GetMenuByIdAsync(id);
                if (menuItem == null)
                {
                    return NotFound(new { Message = "Menu item not found." });
                }
                return Ok(menuItem);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // PUT: api/menu/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenuItem(int id, [FromBody] MenuDto menuDto)
        {
            try
            {
                menuDto.MenuID = id; // Ensure the ID is passed correctly
                var updatedMenuItem = await _menuService.UpdateMenuAsync(menuDto);
                return Ok(updatedMenuItem);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE: api/menu/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            try
            {
                await _menuService.DeleteMenuAsync(id);
                return Ok(new { Message = "Menu item deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}

