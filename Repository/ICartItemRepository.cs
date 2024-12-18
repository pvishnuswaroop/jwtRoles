using QuickServeAPP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickServeAPP.Repository
{
    public interface ICartItemRepository
    {
        Task<IEnumerable<CartItem>> GetAllCartItemsAsync(int cartId);
        Task<CartItem> GetCartItemByIdAsync(int cartItemId);
        Task<CartItem> AddCartItemAsync(CartItem cartItem);
        Task<CartItem> UpdateCartItemAsync(CartItem cartItem);
        Task DeleteCartItemAsync(int cartItemId);
    }
}

