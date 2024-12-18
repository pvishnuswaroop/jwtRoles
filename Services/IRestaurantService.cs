using QuickServeAPP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuickServeAPP.DTOs;

namespace QuickServeAPP.Services
{
    public interface IRestaurantService
    {
        Task<RestaurantDto> CreateRestaurantAsync(RestaurantDto restaurantDto);
        Task<IEnumerable<RestaurantDto>> GetAllRestaurantsAsync();
        Task<RestaurantDto> GetRestaurantByIdAsync(int restaurantId);
        Task<RestaurantDto> UpdateRestaurantAsync(int restaurantId, RestaurantDto restaurantDto);
        Task<bool> DeleteRestaurantAsync(int restaurantId);
        Task<bool> UpdateRestaurantStatusAsync(int restaurantId, bool isActive); // Approve/suspend a restaurant
        bool DoesRestaurantExist(int id);
        List<Restaurant> GetRestaurantsByItem(string itemName);
        IEnumerable<Restaurant> GetTrendingRestaurants();
    }
}


