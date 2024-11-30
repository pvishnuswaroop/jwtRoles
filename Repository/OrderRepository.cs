using Microsoft.EntityFrameworkCore;
using QuickServeAPP.Data;
using QuickServeAPP.Models;
using QuickServeAPP.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickServeAPP.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        // Create a new order
        public async Task<Order> CreateOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        // Get an order by ID
        public async Task<Order> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems) // Include related order items
                .FirstOrDefaultAsync(o => o.OrderID == orderId);
        }

        // Get orders placed by a specific user
        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems) // Include related order items
                .Where(o => o.UserID == userId)
                .ToListAsync();
        }

        // Get orders for a specific restaurant
        public async Task<IEnumerable<Order>> GetOrdersByRestaurantIdAsync(int restaurantId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems) // Include related order items
                .Where(o => o.RestaurantID == restaurantId)
                .ToListAsync();
        }

        // Update an existing order
        public async Task<Order> UpdateOrderAsync(Order order)
        {
            // Validate that the order exists
            var existingOrder = await _context.Orders.FindAsync(order.OrderID);
            if (existingOrder == null)
            {
                throw new Exception($"Order with ID {order.OrderID} not found.");
            }

            // Update the order
            _context.Entry(existingOrder).CurrentValues.SetValues(order);
            await _context.SaveChangesAsync();

            return existingOrder;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }
        

    }
}


