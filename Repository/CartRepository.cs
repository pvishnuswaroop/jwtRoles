using Microsoft.EntityFrameworkCore;
using QuickServeAPP.Data;
using QuickServeAPP.Models;
using QuickServeAPP.Repository;
using System.Linq;
using System.Threading.Tasks;

namespace QuickServeAPP.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;

        public CartRepository(AppDbContext context)
        {
            _context = context;
        }

        // Fetch the cart by User ID, including related cart items and menu items
        public async Task<Cart> GetCartByUserIdAsync(int userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems) // Include related cart items
                .ThenInclude(ci => ci.Menu) // Include related menu items
                .FirstOrDefaultAsync(c => c.UserID == userId && c.IsActive);
        }

        // Add a new cart
        public async Task AddCartAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
        }

        // Save changes to the database
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCartAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();
        }
    }
}

