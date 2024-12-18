using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickServeAPP.DTOs;
using QuickServeAPP.Models;
using QuickServeAPP.Services;

namespace QuickServeAPP.Controllers
{
    //[Authorize(Roles = "Customer")]
    [ApiController]
    [Route("api/customer/dashboard")]
    public class CustomerDashboardController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly IRestaurantService _restaurantService;
        private readonly IMenuService _menuService;
        private readonly IPaymentService _paymentService;
        private readonly EmailService _emailService;
        private readonly IUserService _userService;

        public CustomerDashboardController(IOrderService orderService, ICartService cartService, IRestaurantService restaurantService, IMenuService menuService, IPaymentService paymentService, IUserService userService, EmailService emailService)
        {
            _orderService = orderService;
            _cartService = cartService;
            _restaurantService = restaurantService;
            _menuService = menuService;
            _paymentService = paymentService;
            _userService = userService;
            _emailService = emailService;
        }

        // Get all restaurants
        [HttpGet("restaurants")]
        public async Task<IActionResult> GetAllRestaurants()
        {
            var restaurants = await _restaurantService.GetAllRestaurantsAsync();
            return Ok(restaurants);
        }

        // Get menu for the restaurant
        [HttpGet("menu")]
        public async Task<IActionResult> GetMenu(int restaurantId)
        {

            try
            {
                var menuItems = await _menuService.GetMenusByRestaurantIdAsync(restaurantId);
                return Ok(menuItems);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        [HttpPost("order/place-order")]
        public async Task<IActionResult> PlaceOrder(int userId, [FromBody] InitializeOrderDto orderDto)
        {
            try
            {
                var order = await _orderService.InitializeOrderFromCartAsync(userId, orderDto.Address);
                if (order != null)
                {
                    var emailBody = $"<h1>Order Confirmation</h1><p>Your order #{order.OrderID} has been placed successfully!</p><p>Total: {order.TotalAmount:C}</p>.<p>Thank you for using QuickServe.</p>";
                    var user = await _userService.GetUserByIdAsync(order.UserID);
                    var userEmail = user?.Email;
                    await _emailService.SendEmailAsync(userEmail, "Order Confirmation", emailBody);
                    return Ok(order);
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // Get order history
        [HttpGet("orders")]
        public async Task<IActionResult> GetOrderHistory(int userId)
        {
            try
            {
                var orders = await _orderService.GetOrdersByUserIdAsync(userId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // Get current cart
        [HttpGet("cart")]
        public async Task<IActionResult> GetCart(int userId)
        {
            try
            {
                var cart = await _cartService.GetOrCreateCartByUserIdAsync(userId);
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // Add an item to the cart
        [HttpPost("cart/item")]
        public async Task<IActionResult> AddCartItem(int userId, [FromBody] CartItemDto cartItemDto)
        {
            try
            {
                var updatedCart = await _cartService.AddItemToCartAsync(userId, cartItemDto);
                return Ok(updatedCart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // Remove an item from the cart
        [HttpDelete("cart/item/{itemId}")]
        public async Task<IActionResult> RemoveCartItem(int userId, int menuId)
        {
            try
            {
                var updatedCart = await _cartService.RemoveItemFromCartAsync(userId, menuId);
                return Ok(updatedCart);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("Payments/{userId}")]
        public async Task<IActionResult> GetPaymentsAndPendingOrdersByUserId(int userId)
        {
            try
            {
                var payments = await _paymentService.GetPaymentsAndPendingOrdersByUserIdAsync(userId);
                return Ok(payments);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        [HttpPost("Process-Payment")]
        public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentDto paymentDto)
        {
            try
            {
                var payment = await _paymentService.ProcessPaymentAsync(paymentDto);
                return Ok(payment);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

    }
}
