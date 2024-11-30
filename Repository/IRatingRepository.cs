
using QuickServeAPP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickServeAPP.Repository
{
    public interface IRatingRepository
    {
        Task AddRatingAsync(Rating rating);
        Task<IEnumerable<Rating>> GetRatingsByRestaurantIdAsync(int restaurantId);
        Task<IEnumerable<Rating>> GetRatingsByMenuIdAsync(int menuId);
        Task<decimal> GetAverageRatingByMenuIdAsync(int menuId);
        Task<decimal> GetAverageRatingByRestaurantIdAsync(int restaurantId);
        Task SaveChangesAsync();
        Task<double> GetAverageRatingAsync();
    }
}

