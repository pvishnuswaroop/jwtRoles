// IOrderService.cs
using QuickServeAPP.DTOs;
using QuickServeAPP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickServeAPP.Services
{
    public interface IOrderService
    {
        Task<OrderDto> PlaceOrderAsync(OrderDto orderDto);
        Task<OrderDto> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(int userId);
        Task<IEnumerable<OrderDto>> GetOrdersByRestaurantIdAsync(int restaurantId);
        Task<OrderDto> UpdateOrderStatusAsync(int orderId, string status);
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync(); // Fetch all orders
        Task<OrderDto> InitializeOrderFromCartAsync(int userId, string address);
        Task<IEnumerable<OrderDto>> GetOrdersByStatusAsync(string status);
    }
}


