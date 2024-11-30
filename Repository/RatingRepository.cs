using Microsoft.EntityFrameworkCore;
using QuickServeAPP.Data;
using QuickServeAPP.Models;
using QuickServeAPP.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickServeAPP.Repository
{
    public class RatingRepository : IRatingRepository
    {
        private readonly AppDbContext _context;

        public RatingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddRatingAsync(Rating rating)
        {
            await _context.Ratings.AddAsync(rating);
        }

        public async Task<IEnumerable<Rating>> GetRatingsByMenuIdAsync(int menuId)
        {
            return await _context.Ratings
                .Where(r => r.MenuID == menuId)
                .ToListAsync();
        }

        public async Task<decimal> GetAverageRatingByMenuIdAsync(int menuId)
        {
            var average = await _context.Ratings
                .Where(r => r.MenuID == menuId)
                .AverageAsync(r => (double?)r.RatingScore);

            return (decimal)(average ?? 0.0); // Handle null for no ratings
        }

        public async Task<IEnumerable<Rating>> GetRatingsByRestaurantIdAsync(int restaurantId)
        {
            return await _context.Ratings
                .Where(r => r.RestaurantID == restaurantId)
                .ToListAsync();
        }

        
        public async Task<decimal> GetAverageRatingByRestaurantIdAsync(int restaurantId)
        {
            var average = await _context.Ratings
                .Where(r => r.RestaurantID == restaurantId)
                .AverageAsync(r => (double?)r.RatingScore); // Use nullable double for empty results

            return (decimal)(average ?? 0.0); // Convert to decimal and handle null (no ratings yet)
        }

        public async Task<double> GetAverageRatingAsync()
        {
            return await _context.Ratings.AverageAsync(r => r.RatingScore);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

