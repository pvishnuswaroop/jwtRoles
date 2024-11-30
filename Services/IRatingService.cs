
using QuickServeAPP.DTOs;
using QuickServeAPP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickServeAPP.Services
{
    public interface IRatingService
    {
        Task SubmitRatingAsync(SubmitRatingDto ratingDto);
        Task<IEnumerable<RatingDto>> GetRatingsByRestaurantIdAsync(int restaurantId);
        Task<decimal> GetAverageRatingByRestaurantIdAsync(int restaurantId);
        Task<IEnumerable<RatingDto>> GetRatingsByMenuIdAsync(int menuId);
        Task<decimal> GetAverageRatingByMenuIdAsync(int menuId);
    }
}

