using QuickServeAPP.DTOs;
using QuickServeAPP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickServeAPP.Services
{
    public interface IMenuService
    {
        //Task<IEnumerable<Menu>> GetAllMenusAsync();
        //Task<Menu> GetMenuByIdAsync(int menuId);
        //Task<Menu> CreateMenuAsync(Menu menu);
        //Task<Menu> UpdateMenuAsync(Menu menu);
        //Task DeleteMenuAsync(int menuId);

        Task<MenuDto> CreateMenuAsync(MenuDto menuDto);
        Task<IEnumerable<MenuDto>> GetMenusByRestaurantIdAsync(int restaurantId);
        Task<IEnumerable<MenuWithRatingDto>> GetMenusWithRatingsByRestaurantIdAsync(int restaurantId);
        Task<MenuDto> GetMenuByIdAsync(int menuId);
        Task<MenuDto> UpdateMenuAsync(MenuDto menuDto);
        Task<bool> DeleteMenuAsync(int menuId);
    }
}

