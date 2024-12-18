using Microsoft.EntityFrameworkCore;
using QuickServeAPP.Data;
using QuickServeAPP.Models;
using QuickServeAPP.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuickServeAPP.Repository
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly AppDbContext _context;

        public OrderItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<OrderItem>> GetAllOrderItemsAsync(int orderId)
        {
            return await _context.OrderItems.Where(oi => oi.OrderID == orderId).ToListAsync();
        }

        public async Task<OrderItem> GetOrderItemByIdAsync(int orderItemId)
        {
            return await _context.OrderItems.FirstOrDefaultAsync(oi => oi.OrderItemID == orderItemId);
        }

        public async Task<OrderItem> AddOrderItemAsync(OrderItem orderItem)
        {
            _context.OrderItems.Add(orderItem);
            await _context.SaveChangesAsync();
            return orderItem;
        }

        public async Task<OrderItem> UpdateOrderItemAsync(OrderItem orderItem)
        {
            _context.OrderItems.Update(orderItem);
            await _context.SaveChangesAsync();
            return orderItem;
        }

        public async Task DeleteOrderItemAsync(int orderItemId)
        {
            var orderItem = await GetOrderItemByIdAsync(orderItemId);
            if (orderItem != null)
            {
                _context.OrderItems.Remove(orderItem);
                await _context.SaveChangesAsync();
            }
        }
    }
}

