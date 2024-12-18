using Microsoft.EntityFrameworkCore;
using QuickServeAPP.Data;
using QuickServeAPP.Models;
using QuickServeAPP.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuickServeAPP.DTOs;

namespace QuickServeAPP.Repository
{
    public class MenuRepository : IMenuRepository
    {
        private readonly AppDbContext _context;

        public MenuRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Menu>> GetMenusByRestaurantIdAsync(int restaurantId)
        {
            // Use LINQ to query the database
            return await _context.Menus
                .Where(m => m.RestaurantID == restaurantId && !m.IsDeleted) // Filter by restaurant ID
                .ToListAsync(); // Execute query and convert to a list
        }


        public async Task<IEnumerable<MenuWithRatingDto>> GetMenusWithRatingsByRestaurantIdAsync(int restaurantId)
        {
            var menus = await _context.Menus
                .Where(m => m.RestaurantID == restaurantId && !m.IsDeleted)
                .Select(m => new MenuWithRatingDto
                {
                    MenuID = m.MenuID,
                    ItemName = m.ItemName,
                    Description = m.Description,
                    Category = m.Category,
                    Price = m.Price,
                    AvailabilityTime = m.AvailabilityTime,
                    DietaryInfo = m.DietaryInfo,
                    Status = m.Status.ToString(),
                    AverageRating = _context.Ratings
                        .Where(r => r.MenuID == m.MenuID)
                        .Average(r => (double?)r.RatingScore) ?? 0.0 // Calculate average rating or default to 0
                })
                .OrderByDescending(m => m.AverageRating) // Sort by average rating
                .ToListAsync();

            return menus;
        }



        public async Task<Menu> GetMenuByIdAsync(int menuId)
        {
            return await _context.Menus.Include(m => m.Restaurant).FirstOrDefaultAsync(m => m.MenuID == menuId);
        }

        public async Task<Menu> CreateMenuAsync(Menu menu)
        {
            _context.Menus.Add(menu);
            await _context.SaveChangesAsync();
            return menu;
        }

        public async Task<Menu> UpdateMenuAsync(Menu menu)
        {
            _context.Menus.Update(menu);
            await _context.SaveChangesAsync();
            return menu;
        }

        public async Task<bool> DeleteMenuAsync(int menuId)
        {
            var menu = await GetMenuByIdAsync(menuId);
            if (menu == null)
            { return false; }
            menu.IsDeleted = true; // Soft delete by marking it as deleted
            _context.Menus.Update(menu); // Mark the menu as modified
            await _context.SaveChangesAsync(); // Save changes
            return true;

        }

        public async Task<List<Menu>> GetMenusByIdsAsync(List<int> menuIds)
        {
            return await _context.Menus
                .Where(menu => menuIds.Contains(menu.MenuID))
                .ToListAsync();
        }

    }
}

