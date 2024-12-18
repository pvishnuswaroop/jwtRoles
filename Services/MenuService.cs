using QuickServeAPP.Repository;
using QuickServeAPP.Models;
using QuickServeAPP.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuickServeAPP.DTOs;

namespace QuickServeAPP.Services
{
    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;

        public MenuService(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public async Task<MenuDto> CreateMenuAsync(MenuDto menuDto)
        {
            //// Validate the enum value
            //if (!Enum.IsDefined(typeof(MenuItemStatus), menuDto.Status))
            //{
            //    throw new Exception($"Invalid status value: {menuDto.Status}");
            //}

            // Map DTO to Entity
            var menu = new Menu
            {
                RestaurantID = menuDto.RestaurantID,
                ItemName = menuDto.ItemName,
                Description = menuDto.Description,
                Category = menuDto.Category,
                Price = menuDto.Price,
                AvailabilityTime = menuDto.AvailabilityTime,
                DietaryInfo = menuDto.DietaryInfo,
                Status = menuDto.Status
            };

            // Save to the database
            var createdMenu = await _menuRepository.CreateMenuAsync(menu);

            // Map Entity back to DTO
            return new MenuDto
            {
                MenuID = createdMenu.MenuID,
                RestaurantID = createdMenu.RestaurantID,
                ItemName = createdMenu.ItemName,
                Description = createdMenu.Description,
                Category = createdMenu.Category,
                Price = createdMenu.Price,
                AvailabilityTime = createdMenu.AvailabilityTime,
                DietaryInfo = createdMenu.DietaryInfo,
                Status = createdMenu.Status
            };
        }


        public async Task<IEnumerable<MenuDto>> GetMenusByRestaurantIdAsync(int restaurantId)
        {
            var menus = await _menuRepository.GetMenusByRestaurantIdAsync(restaurantId);

            // Map each Entity to DTO
            return menus.Select(m => new MenuDto
            {
                MenuID = m.MenuID,
                RestaurantID = m.RestaurantID,
                ItemName = m.ItemName,
                Description = m.Description,
                Category = m.Category,
                Price = m.Price,
                AvailabilityTime = m.AvailabilityTime,
                DietaryInfo = m.DietaryInfo,
                Status = m.Status
            }).ToList();
        }

        public async Task<IEnumerable<MenuWithRatingDto>> GetMenusWithRatingsByRestaurantIdAsync(int restaurantId)
        {
            return await _menuRepository.GetMenusWithRatingsByRestaurantIdAsync(restaurantId);
        }


        public async Task<MenuDto> GetMenuByIdAsync(int menuId)
        {
            var menu = await _menuRepository.GetMenuByIdAsync(menuId);
            if (menu == null)
            {
                throw new Exception("Menu item not found.");
            }

            // Map Entity to DTO
            return new MenuDto
            {
                MenuID = menu.MenuID,
                RestaurantID = menu.RestaurantID,
                ItemName = menu.ItemName,
                Description = menu.Description,
                Category = menu.Category,
                Price = menu.Price,
                AvailabilityTime = menu.AvailabilityTime,
                DietaryInfo = menu.DietaryInfo,
                Status = menu.Status
            };
        }

        public async Task<MenuDto> UpdateMenuAsync(MenuDto menuDto)
        {
            // Fetch existing menu
            var menu = await _menuRepository.GetMenuByIdAsync(menuDto.MenuID);
            if (menu == null)
            {
                throw new Exception("Menu item not found.");
            }

            // Update Entity
            menu.ItemName = menuDto.ItemName;
            menu.Description = menuDto.Description;
            menu.Category = menuDto.Category;
            menu.Price = menuDto.Price;
            menu.AvailabilityTime = menuDto.AvailabilityTime;
            menu.DietaryInfo = menuDto.DietaryInfo;
            menu.Status = menuDto.Status;

            // Save changes
            var updatedMenu = await _menuRepository.UpdateMenuAsync(menu);

            // Map Entity to DTO
            return new MenuDto
            {
                MenuID = updatedMenu.MenuID,
                RestaurantID = updatedMenu.RestaurantID,
                ItemName = updatedMenu.ItemName,
                Description = updatedMenu.Description,
                Category = updatedMenu.Category,
                Price = updatedMenu.Price,
                AvailabilityTime = updatedMenu.AvailabilityTime,
                DietaryInfo = updatedMenu.DietaryInfo,
                Status = updatedMenu.Status
            };
        }

        public async Task<bool> DeleteMenuAsync(int menuId)
        {
             await _menuRepository.DeleteMenuAsync(menuId);
            return true;
        }
    }
}


