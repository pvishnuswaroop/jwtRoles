using QuickServeAPP.DTOs;
using QuickServeAPP.Models;
using QuickServeAPP.Repository;

namespace QuickServeAPP.Services
{
    public class ReportService:IReportService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IRatingRepository _ratingRepository;

        public ReportService(IUserRepository userRepository, IRestaurantRepository restaurantRepository, IOrderRepository orderRepository, IRatingRepository ratingRepository)
        {
            _userRepository = userRepository;
            _restaurantRepository = restaurantRepository;
            _orderRepository = orderRepository;
            _ratingRepository = ratingRepository;
        }

        public async Task<SystemReportDto> GenerateSystemReportAsync()
        {
            var totalUsers = (await _userRepository.GetAllUsersAsync()).Count();
            var activeUsers = (await _userRepository.GetAllUsersAsync()).Count(u => u.IsActive);
            var InactiveUsers = (await _userRepository.GetAllUsersAsync()).Count(u => !u.IsActive);

            var totalRestaurants = (await _restaurantRepository.GetAllRestaurantsAsync()).Count();
            var activeRestaurants = (await _restaurantRepository.GetAllRestaurantsAsync()).Count(r => r.IsActive);
            var suspendedRestaurants = totalRestaurants - activeRestaurants;

            var totalOrders = (await _orderRepository.GetAllOrdersAsync()).Count();
            var pendingOrders = (await _orderRepository.GetAllOrdersAsync()).Count(o => o.OrderStatus == OrderStatus.Pending);
            var completedOrders = (await _orderRepository.GetAllOrdersAsync()).Count(o => o.OrderStatus == OrderStatus.Completed);

            var totalRevenue = (await _orderRepository.GetAllOrdersAsync())
                .Where(o => o.OrderStatus == OrderStatus.Completed)
                .Sum(o => o.TotalAmount);

            var averageRating = await _ratingRepository.GetAverageRatingAsync();

            return new SystemReportDto
            {
                TotalUsers = totalUsers,
                ActiveUsers = activeUsers,
                SuspendedUsers = InactiveUsers,
                TotalRestaurants = totalRestaurants,
                ActiveRestaurants = activeRestaurants,
                SuspendedRestaurants = suspendedRestaurants,
                TotalOrders = totalOrders,
                PendingOrders = pendingOrders,
                CompletedOrders = completedOrders,
                TotalRevenue = totalRevenue,
                AverageRating = averageRating
            };
        }

    }
}
