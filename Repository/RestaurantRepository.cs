using Microsoft.EntityFrameworkCore;
using QuickServeAPP.Data;
using QuickServeAPP.Models;
using QuickServeAPP.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickServeAPP.Repository
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly AppDbContext _context;

        public RestaurantRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync()
        {
            return await _context.Restaurants.ToListAsync();
        }

        public async Task<Restaurant> GetRestaurantByIdAsync(int restaurantId)
        {
            return await _context.Restaurants.Include(r => r.Menus).FirstOrDefaultAsync(r => r.RestaurantID == restaurantId);
        }

        public async Task<Restaurant> CreateRestaurantAsync(Restaurant restaurant)
        {
            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();
            return restaurant;
        }

        public async Task<Restaurant> UpdateRestaurantAsync(Restaurant restaurant)
        {
            _context.Restaurants.Update(restaurant);
            await _context.SaveChangesAsync();
            return restaurant;
        }

        public async Task<bool> DeleteRestaurantAsync(int restaurantId)
        {
            var restaurant = await GetRestaurantByIdAsync(restaurantId);
            if (restaurant == null)
            { return false; }

                _context.Restaurants.Remove(restaurant);
                await _context.SaveChangesAsync();
            return true;
            
        }

        public Restaurant GetById(int id)
        {
            return _context.Restaurants.FirstOrDefault(r => r.RestaurantID == id);
        }

        public List<Restaurant> GetRestaurantsByItem(string itemName)
        {
            return _context.Restaurants
                .Where(r => r.Menus.Any(mi => mi.ItemName.Contains(itemName)))
                .ToList();
        }

        public IEnumerable<Restaurant> GetTrendingRestaurants()
        {
            // Fetch the first three restaurants (or random ones if needed)
            //return _context.Restaurants
            //               .OrderBy(r => r.RestaurantID) // Order by ID (or another property)
            //               .Take(3) // Fetch the first 3 records
            //               .ToList();

            // For random restaurants, you can shuffle the results:
            var restaurants = _context.Restaurants.OrderBy(r => Guid.NewGuid()).Take(3).ToList();
            var random = new Random();
            foreach (var restaurant in restaurants)
            {
                restaurant.Rating = Math.Round(4 + random.NextDouble(), 1); 
                restaurant.Label = random.Next(0, 2) == 0 ? "Popular" : "Recommended"; 
            }

            return restaurants;
        }



    }
}

