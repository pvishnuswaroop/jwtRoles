
using System.Collections.Generic;
using System.Threading.Tasks;
using QuickServeAPP.DTOs;
using QuickServeAPP.Models;
using QuickServeAPP.Repository;
using QuickServeAPP.Services;

namespace QuickServeAPP.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IOrderRepository _orderRepository;

        public RatingService(IRatingRepository ratingRepository, IOrderRepository orderRepository)
        {
            _ratingRepository = ratingRepository;
            _orderRepository = orderRepository;
        }

        public async Task SubmitRatingAsync(SubmitRatingDto ratingDto)
        {
            var order = await _orderRepository.GetOrderByIdAsync(ratingDto.OrderID.Value);

            // Optional: Validate that the user has ordered from the restaurant
            if (ratingDto.OrderID.HasValue)
            {
                
                if (order == null || order.UserID != ratingDto.UserID || order.RestaurantID != ratingDto.RestaurantID)
                {
                    throw new Exception("Invalid order for rating.");
                }
            }

            // Create a rating for the order
            var orderRating = new Rating
            {
                UserID = ratingDto.UserID,
                RestaurantID = order.RestaurantID,
                OrderID = order.OrderID,
                RatingScore = ratingDto.RatingScore,
                ReviewText = ratingDto.ReviewText,
                RatingDate = DateTime.UtcNow
            };

            await _ratingRepository.AddRatingAsync(orderRating);

            // Distribute the rating to menu items in the order
            foreach (var orderItem in order.OrderItems)
            {
                var menuItemRating = new Rating
                {
                    UserID = ratingDto.UserID,
                    RestaurantID = order.RestaurantID,
                    MenuID = orderItem.MenuID,
                    RatingScore = ratingDto.RatingScore, // Assign the same rating to the menu item
                    RatingDate = DateTime.UtcNow
                };

                await _ratingRepository.AddRatingAsync(menuItemRating);
            }

            await _ratingRepository.SaveChangesAsync();

        }

        public async Task<IEnumerable<RatingDto>> GetRatingsByRestaurantIdAsync(int restaurantId)
        {
            var ratings = await _ratingRepository.GetRatingsByRestaurantIdAsync(restaurantId);

            return ratings.Select(r => new RatingDto
            {
                RatingID = r.RatingID,
                UserID = r.UserID,
                RestaurantID = r.RestaurantID,
                MenuID = r.MenuID,
                OrderID = r.OrderID,
                RatingScore = r.RatingScore,
                ReviewText = r.ReviewText,
                RatingDate = r.RatingDate
            });
        }

        public async Task<decimal> GetAverageRatingByRestaurantIdAsync(int restaurantId)
        {
            return await _ratingRepository.GetAverageRatingByRestaurantIdAsync(restaurantId);
        }

        public async Task<IEnumerable<RatingDto>> GetRatingsByMenuIdAsync(int menuId)
        {
            var ratings = await _ratingRepository.GetRatingsByMenuIdAsync(menuId);

            return ratings.Select(r => new RatingDto
            {
                RatingID = r.RatingID,
                UserID = r.UserID,
                RestaurantID = r.RestaurantID,
                MenuID = r.MenuID,
                RatingScore = r.RatingScore,
                ReviewText = r.ReviewText,
                RatingDate = r.RatingDate
            });
        }

        public async Task<decimal> GetAverageRatingByMenuIdAsync(int menuId)
        {
            return await _ratingRepository.GetAverageRatingByMenuIdAsync(menuId);
        }
    }
}

