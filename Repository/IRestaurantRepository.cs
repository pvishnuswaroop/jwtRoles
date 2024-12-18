using QuickServeAPP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickServeAPP.Repository
{
    public interface IRestaurantRepository
    {
        Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync();
        Task<Restaurant> GetRestaurantByIdAsync(int restaurantId);
        Task<Restaurant> CreateRestaurantAsync(Restaurant restaurant);
        Task<Restaurant> UpdateRestaurantAsync(Restaurant restaurant);
        Task<bool> DeleteRestaurantAsync(int restaurantId);

        Restaurant GetById(int id);
        List<Restaurant> GetRestaurantsByItem(string itemName);
        IEnumerable<Restaurant> GetTrendingRestaurants();
    }
}

