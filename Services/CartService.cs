
using System.Threading.Tasks;
using QuickServeAPP.DTOs;
using QuickServeAPP.Models;
using QuickServeAPP.Repository;
using QuickServeAPP.Services;

namespace QuickServeAPP.Services
{
    // CartService.cs
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMenuRepository _menuRepository;
        private readonly IUserRepository _userRepository;

        public CartService(ICartRepository cartRepository, IMenuRepository menuRepository, IUserRepository userRepository)
        {
            _cartRepository = cartRepository;
            _menuRepository = menuRepository;
            _userRepository = userRepository;
        }

        public async Task<CartDto> GetOrCreateCartByUserIdAsync(int userId)
        {
            // Check if the user exists
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

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

            // Return the cart (map it to CartDto)
            return new CartDto
            {
                CartID = cart.CartID,
                UserID = cart.UserID,
                CreationDate = cart.CreationDate,
                IsActive = cart.IsActive,
                CartItems = cart.CartItems.Select(item => new CartItemDto
                {
                    MenuID = item.MenuID,
                    Quantity = item.Quantity,
                    Price = item.Menu.Price // Fetch price dynamically from the menu
                }).ToList()
            };
        }


        public async Task<CartDto> AddItemToCartAsync(int userId, CartItemDto cartItemDto)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) throw new Exception("User not found.");

            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null) throw new Exception("Cart not found.");

            var menuItem = await _menuRepository.GetMenuByIdAsync(cartItemDto.MenuID);
            if (menuItem == null) throw new Exception("Menu item not found.");

            // Add the item to the cart
            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.MenuID == cartItemDto.MenuID);
            if (existingItem != null)
            {
                existingItem.Quantity += cartItemDto.Quantity;  // Update quantity if item already exists
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    MenuID = cartItemDto.MenuID,
                    Quantity = cartItemDto.Quantity
                });
            }

            await _cartRepository.SaveChangesAsync();

            return await GetOrCreateCartByUserIdAsync(userId);  // Return updated cart
        }

        public async Task<CartDto> RemoveItemFromCartAsync(int userId, int menuId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null) throw new Exception("Cart not found.");

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.MenuID == menuId);
            if (cartItem == null) throw new Exception("Item not found in cart.");

            cart.CartItems.Remove(cartItem);
            await _cartRepository.SaveChangesAsync();

            return await GetOrCreateCartByUserIdAsync(userId);  // Return updated cart
        }

        public async Task<CartDto> UpdateItemQuantityAsync(int userId, int menuId, int quantity)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null) throw new Exception("Cart not found.");

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.MenuID == menuId);
            if (cartItem == null) throw new Exception("Item not found in cart.");

            cartItem.Quantity = quantity;
            await _cartRepository.SaveChangesAsync();

            return await GetOrCreateCartByUserIdAsync(userId);  // Return updated cart
        }

        public async Task<bool> ClearCartAsync(int userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null) throw new Exception("Cart not found.");

            cart.CartItems.Clear();
            await _cartRepository.SaveChangesAsync();

            return true;
        }
    }

}

