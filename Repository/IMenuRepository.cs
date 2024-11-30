
using QuickServeAPP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuickServeAPP.DTOs;

namespace QuickServeAPP.Repository
{
    public interface IMenuRepository
    {
        Task<IEnumerable<Menu>> GetMenusByRestaurantIdAsync(int restaurantId);
        Task<IEnumerable<MenuWithRatingDto>> GetMenusWithRatingsByRestaurantIdAsync(int restaurantId);
        Task<Menu> GetMenuByIdAsync(int menuId);
        Task<Menu> CreateMenuAsync(Menu menu);
        Task<Menu> UpdateMenuAsync(Menu menu);
        Task<bool> DeleteMenuAsync(int menuId);
        Task<List<Menu>> GetMenusByIdsAsync(List<int> menuIds);
    }
}

