using QuickServeAPP.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace QuickServeAPP.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemID { get; set; }

        [Required(ErrorMessage = "Cart ID is required.")]
        public int CartID { get; set; }  // Foreign Key to Cart

        [Required(ErrorMessage = "Menu ID is required.")]
        public int MenuID { get; set; }  // Foreign Key to Menu

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
        public int Quantity { get; set; }

        //// Optional: Store the price at the time the item was added to the cart
        //[Required(ErrorMessage = "Price is required.")]
        //[Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        //public decimal Price { get; set; }

        // Navigation properties
        public virtual Cart Cart { get; set; }  // Many-to-one with Cart
        public virtual Menu Menu { get; set; }  // Many-to-one with Menu
    }
}

