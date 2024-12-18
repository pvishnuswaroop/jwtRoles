
using QuickServeAPP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickServeAPP.Repository
{
    public interface IOrderItemRepository
    {
        Task<IEnumerable<OrderItem>> GetAllOrderItemsAsync(int orderId);
        Task<OrderItem> GetOrderItemByIdAsync(int orderItemId);
        Task<OrderItem> AddOrderItemAsync(OrderItem orderItem);
        Task<OrderItem> UpdateOrderItemAsync(OrderItem orderItem);
        Task DeleteOrderItemAsync(int orderItemId);
    }
}

