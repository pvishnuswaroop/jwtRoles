
using QuickServeAPP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickServeAPP.Repository
{
    public interface ICartRepository
    {
        Task<Cart> GetCartByUserIdAsync(int userId);
        Task AddCartAsync(Cart cart);
        Task SaveChangesAsync();
        Task UpdateCartAsync(Cart cart);
    }
}

