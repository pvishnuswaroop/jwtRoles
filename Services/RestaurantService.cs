using QuickServeAPP.DTOs;
using QuickServeAPP.Models;
using QuickServeAPP.Repository;
using QuickServeAPP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickServeAPP.Services
{
    public class RestaurantService : IRestaurantService
    {
        private readonly IRestaurantRepository _restaurantRepository;

        public RestaurantService(IRestaurantRepository restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }

        public async Task<RestaurantDto> CreateRestaurantAsync(RestaurantDto restaurantDto)
        {
            // Check if a restaurant with the same name and location already exists
            var existingRestaurant = (await _restaurantRepository.GetAllRestaurantsAsync())
                .FirstOrDefault(r => r.Name.Equals(restaurantDto.Name, StringComparison.OrdinalIgnoreCase)
                                     && r.Location.Equals(restaurantDto.Location, StringComparison.OrdinalIgnoreCase));
            if (existingRestaurant != null)
            {
                throw new Exception("A restaurant with the same name and location already exists.");
            }

            // Map DTO to Entity
            var restaurant = new Restaurant
            {
                Name = restaurantDto.Name,
                Location = restaurantDto.Location,
                PhoneNumber = restaurantDto.PhoneNumber,
                IsActive = restaurantDto.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Save to database
            var createdRestaurant = await _restaurantRepository.CreateRestaurantAsync(restaurant);

            // Map Entity back to DTO
            return new RestaurantDto
            {
                //RestaurantID = createdRestaurant.RestaurantID,
                Name = createdRestaurant.Name,
                Location = createdRestaurant.Location,
                PhoneNumber = createdRestaurant.PhoneNumber,
                IsActive = createdRestaurant.IsActive,
                CreatedAt = createdRestaurant.CreatedAt,
                UpdatedAt = createdRestaurant.UpdatedAt
            };
        }

        public async Task<IEnumerable<RestaurantDto>> GetAllRestaurantsAsync()
        {
            var restaurants = await _restaurantRepository.GetAllRestaurantsAsync();

            // Map each entity to DTO
            return restaurants.Select(r => new RestaurantDto
            {
                RestaurantID = r.RestaurantID,
                Name = r.Name,
                Location = r.Location,
                PhoneNumber = r.PhoneNumber,
                IsActive = r.IsActive,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            }).ToList();
        }

        public async Task<RestaurantDto> GetRestaurantByIdAsync(int restaurantId)
        {
            var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(restaurantId);

            if (restaurant == null)
            {
                throw new Exception("Restaurant not found.");
            }

            // Map Entity to DTO
            return new RestaurantDto
            {
                RestaurantID = restaurant.RestaurantID,
                Name = restaurant.Name,
                Location = restaurant.Location,
                PhoneNumber = restaurant.PhoneNumber,
                IsActive = restaurant.IsActive,
                CreatedAt = restaurant.CreatedAt,
                UpdatedAt = restaurant.UpdatedAt
            };
        }

        public async Task<RestaurantDto> UpdateRestaurantAsync(int restaurantId, RestaurantDto restaurantDto)
        {
            // Fetch existing restaurant
            var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(restaurantId);

            if (restaurant == null)
            {
                throw new Exception("Restaurant not found.");
            }

            // Update entity
            restaurant.Name = restaurantDto.Name;
            restaurant.Location = restaurantDto.Location;
            restaurant.PhoneNumber = restaurantDto.PhoneNumber;
            restaurant.IsActive = restaurantDto.IsActive;
            restaurant.UpdatedAt = DateTime.UtcNow;

            // Save changes
            var updatedRestaurant = await _restaurantRepository.UpdateRestaurantAsync(restaurant);

            // Map updated entity to DTO
            return new RestaurantDto
            {
                RestaurantID = updatedRestaurant.RestaurantID,
                Name = updatedRestaurant.Name,
                Location = updatedRestaurant.Location,
                PhoneNumber = updatedRestaurant.PhoneNumber,
                IsActive = updatedRestaurant.IsActive,
                CreatedAt = updatedRestaurant.CreatedAt,
                UpdatedAt = updatedRestaurant.UpdatedAt
            };
        }

        public async Task<bool> DeleteRestaurantAsync(int restaurantId)
        {
            var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(restaurantId);
            if (restaurant == null)
             return false; 
            return await _restaurantRepository.DeleteRestaurantAsync(restaurantId);
        }

        // Approve or suspend a restaurant
        public async Task<bool> UpdateRestaurantStatusAsync(int restaurantId, bool isActive)
        {
            var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(restaurantId);
            if (restaurant == null)
                return false;

            restaurant.IsActive = isActive;
            await _restaurantRepository.UpdateRestaurantAsync(restaurant);
            return true;
        }

        public bool DoesRestaurantExist(int id)
        {
            return _restaurantRepository.GetById(id) != null;
        }

        public List<Restaurant> GetRestaurantsByItem(string itemName)
        {
            return _restaurantRepository.GetRestaurantsByItem(itemName);
        }

        public IEnumerable<Restaurant> GetTrendingRestaurants()
        {
            return _restaurantRepository.GetTrendingRestaurants();
        }


    }
}


