using QuickServeAPP.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuickServeAPP.Models
{
    public class Cart
    {
        [Key]
        public int CartID { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public int UserID { get; set; }  // Foreign Key for User

        [Required(ErrorMessage = "Creation date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;  // Use UTC now for consistency

        [Required]
        public bool IsActive { get; set; } = true;  // Indicates if the cart is currently active

        // Navigation properties
        public virtual User User { get; set; }  // Many-to-one relationship with User (non-nullable)
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();  // One-to-many with CartItem
    }
}


