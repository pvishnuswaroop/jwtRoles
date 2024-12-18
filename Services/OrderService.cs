// OrderService.cs
using QuickServeAPP.DTOs;
using QuickServeAPP.Models;
using QuickServeAPP.Repository;

namespace QuickServeAPP.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly ICartRepository _cartRepository;
        private readonly ICartService _cartService;

        public OrderService(IOrderRepository orderRepository, IMenuRepository menuRepository, IUserRepository userRepository, IRestaurantRepository restaurantRepository, ICartRepository cartRepository, ICartService cartService)
        {
            _orderRepository = orderRepository;
            _menuRepository = menuRepository;
            _userRepository = userRepository;
            _restaurantRepository = restaurantRepository;
            _cartRepository = cartRepository;
            _cartService = cartService;
        }

        public async Task<OrderDto> PlaceOrderAsync(OrderDto orderDto)
        {
            // Validate if the address is provided
            if (string.IsNullOrWhiteSpace(orderDto.Address))
            {
                throw new Exception("Delivery address is required to place an order.");
            }

            // Validate user
            var user = await _userRepository.GetUserByIdAsync(orderDto.UserID);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            // Validate restaurant
            var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(orderDto.RestaurantID);
            if (restaurant == null)
            {
                throw new Exception("Restaurant not found.");
            }

            // Validate and calculate total amount for each item
            decimal totalAmount = 0;
            var orderItems = new List<OrderItem>();
            foreach (var item in orderDto.OrderItems)
            {
                var menuItem = await _menuRepository.GetMenuByIdAsync(item.MenuID);
                if (menuItem == null)
                {
                    throw new Exception($"Menu item with ID {item.MenuID} not found.");
                }

                // Calculate the total price for this item (quantity * price)
                decimal itemTotal = menuItem.Price * item.Quantity;
                totalAmount += itemTotal;

                orderItems.Add(new OrderItem
                {
                    MenuID = menuItem.MenuID,
                    Quantity = item.Quantity,
                    Price = menuItem.Price
                });
            }

            // Create the order entity
            var order = new Order
            {
                UserID = orderDto.UserID,
                RestaurantID = orderDto.RestaurantID,
                Address = orderDto.Address,
                OrderDate = DateTime.UtcNow,
                OrderStatus = OrderStatus.Pending,  // Default status
                TotalAmount = totalAmount,
                OrderItems = orderItems
            };

            // Save the order to the database
            var createdOrder = await _orderRepository.CreateOrderAsync(order);

            // Map the created order entity back to the OrderDto
            return new OrderDto
            {
                OrderID = createdOrder.OrderID,
                UserID = createdOrder.UserID,
                RestaurantID = createdOrder.RestaurantID,
                Address = createdOrder.Address,
                OrderDate = createdOrder.OrderDate,
                OrderStatus = createdOrder.OrderStatus.ToString(),
                TotalAmount = createdOrder.TotalAmount,
                OrderItems = createdOrder.OrderItems.Select(item => new OrderItemDto
                {
                    MenuID = item.MenuID,
                    Quantity = item.Quantity
                }).ToList()
            };
        }

        public async Task<OrderDto> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                return null;
            }

            return new OrderDto
            {
                OrderID = order.OrderID,
                UserID = order.UserID,
                RestaurantID = order.RestaurantID,
                Address = order.Address,
                OrderDate = order.OrderDate,
                OrderStatus = order.OrderStatus.ToString(),
                TotalAmount = order.TotalAmount,
                OrderItems = order.OrderItems.Select(item => new OrderItemDto
                {
                    MenuID = item.MenuID,
                    Quantity = item.Quantity
                }).ToList()
            };
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            return orders.Select(order => new OrderDto
            {
                OrderID = order.OrderID,
                UserID = order.UserID,
                RestaurantID = order.RestaurantID,
                Address = order.Address,
                OrderDate = order.OrderDate,
                OrderStatus = order.OrderStatus.ToString(),
                TotalAmount = order.TotalAmount,
                OrderItems = order.OrderItems.Select(item => new OrderItemDto
                {
                    MenuID = item.MenuID,
                    Quantity = item.Quantity
                }).ToList()
            });
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByRestaurantIdAsync(int restaurantId)
        {
            var orders = await _orderRepository.GetOrdersByRestaurantIdAsync(restaurantId);
            return orders.Select(order => new OrderDto
            {
                OrderID = order.OrderID,
                UserID = order.UserID,
                RestaurantID = order.RestaurantID,
                Address = order.Address,
                OrderDate = order.OrderDate,
                OrderStatus = order.OrderStatus.ToString(),
                TotalAmount = order.TotalAmount,
                OrderItems = order.OrderItems.Select(item => new OrderItemDto
                {
                    MenuID = item.MenuID,
                    Quantity = item.Quantity
                }).ToList()
            });
        }

        public async Task<OrderDto> UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
            {
                throw new Exception("Order not found.");
            }

            // Validate the input string and parse it to the enum
            if (!Enum.TryParse<OrderStatus>(status, true, out var newStatus))
            {
                throw new ArgumentException($"Invalid order status: {status}");
            }
            // Update the status
            order.OrderStatus = newStatus;

            var updatedOrder = await _orderRepository.UpdateOrderAsync(order);

            return new OrderDto
            {
                OrderID = updatedOrder.OrderID,
                UserID = updatedOrder.UserID,
                RestaurantID = updatedOrder.RestaurantID,
                Address = updatedOrder.Address,
                OrderDate = updatedOrder.OrderDate,
                OrderStatus = updatedOrder.OrderStatus.ToString(),
                TotalAmount = updatedOrder.TotalAmount,
                OrderItems = updatedOrder.OrderItems.Select(item => new OrderItemDto
                {
                    MenuID = item.MenuID,
                    Quantity = item.Quantity
                }).ToList()
            };
        }

        // Fetch all orders
        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();
            return orders.Select(order => new OrderDto
            {
                OrderID = order.OrderID,
                UserID = order.UserID,
                RestaurantID = order.RestaurantID,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                OrderStatus = order.OrderStatus.ToString(),
                Address = order.Address
            });
        }


        public async Task<IEnumerable<OrderDto>> GetOrdersByStatusAsync(string status)
        {
            var orders = await _orderRepository.GetOrdersByStatusAsync(status);

            // Map the entity to DTO
            return orders.Select(order => new OrderDto
            {
                OrderID = order.OrderID,
                UserID = order.UserID,
                RestaurantID = order.RestaurantID,
                Address = order.Address,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                OrderStatus = order.OrderStatus.ToString(),
                OrderItems = order.OrderItems.Select(item => new OrderItemDto
                {
                    MenuID = item.MenuID,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            });
        }



        public async Task<OrderDto> InitializeOrderFromCartAsync(int userId, string address)
        {
            // Validate the address
            if (string.IsNullOrWhiteSpace(address))
                throw new Exception("Address cannot be empty.");

            // Fetch the existing cart
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);

            // If no cart exists, create a new one
            if (cart == null)
            {
                cart = new Cart
                {
                    UserID = userId,
                    IsActive = true,
                    CreationDate = DateTime.UtcNow
                };

                await _cartRepository.AddCartAsync(cart);
                await _cartRepository.SaveChangesAsync();
            }

            // Fetch the menu details for all MenuIDs in the cart
            var menuIds = cart.CartItems.Select(ci => ci.MenuID).ToList();
            var menus = await _menuRepository.GetMenusByIdsAsync(menuIds);

            if (menus.Count != menuIds.Count)
                throw new Exception("Some menu items are missing or invalid.");

            // Validate cart consistency
            var restaurantId = menus.First().RestaurantID; // Assuming all items belong to the same restaurant
            if (menus.Any(menu => menu.RestaurantID != restaurantId))
                throw new Exception("All items in the cart must belong to the same restaurant.");

            // Calculate the total amount
            var totalAmount = cart.CartItems.Sum(ci =>
            {
                var menu = menus.First(m => m.MenuID == ci.MenuID);
                return menu.Price * ci.Quantity;
            });


            // Create a new Order object
            var order = new Order
            {
                UserID = userId,
                RestaurantID = restaurantId, 
                OrderDate = DateTime.UtcNow,
                OrderStatus = OrderStatus.Pending,
                TotalAmount = totalAmount,
                Address = address,
                OrderItems = cart.CartItems.Select(ci =>
                {
                    var menu = menus.First(m => m.MenuID == ci.MenuID);
                    return new OrderItem
                    {
                        MenuID = ci.MenuID,
                        Quantity = ci.Quantity,
                        Price = menu.Price // Set the price of the item
                    };
                }).ToList()
            };

            // Save the order to the database
            var createdOrder = await _orderRepository.CreateOrderAsync(order);

            // Clear cart after placing order
            cart.CartItems.Clear(); 
            await _cartRepository.UpdateCartAsync(cart);

            // Map to DTO and return
            return new OrderDto
            {
                OrderID = createdOrder.OrderID,
                UserID = createdOrder.UserID,
                RestaurantID = createdOrder.RestaurantID,
                OrderDate = createdOrder.OrderDate,
                TotalAmount = createdOrder.TotalAmount,
                OrderStatus = createdOrder.OrderStatus.ToString(),
                Address = createdOrder.Address,
                OrderItems = createdOrder.OrderItems.Select(oi => new OrderItemDto
                {
                    MenuID = oi.MenuID,
                    Quantity = oi.Quantity,
                    Price = oi.Price
                }).ToList()
            };
        }

    }
}


