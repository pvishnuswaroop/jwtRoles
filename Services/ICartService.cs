using QuickServeAPP.DTOs;
using QuickServeAPP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickServeAPP.Services
{
    public interface ICartService
    {
        Task<CartDto> GetOrCreateCartByUserIdAsync(int userId);
        Task<CartDto> AddItemToCartAsync(int userId, CartItemDto cartItemDto);
        Task<CartDto> RemoveItemFromCartAsync(int userId, int menuId);
        Task<CartDto> UpdateItemQuantityAsync(int userId, int menuId, int quantity);
        Task<bool> ClearCartAsync(int userId);
    }
}

