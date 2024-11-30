using QuickServeAPP.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuickServeAPP.Models
{
    public class Restaurant
    {
        [Key]
        public int RestaurantID { get; set; }

        [Required(ErrorMessage = "Restaurant name is required.")]
        [StringLength(100, ErrorMessage = "Restaurant name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(200, ErrorMessage = "Location cannot exceed 200 characters.")]
        public string Location { get; set; }  // Consider breaking this into separate fields if needed.

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters.")]
        public string PhoneNumber { get; set; }  // Renamed for clarity.

        public bool IsActive { get; set; } = true;  // Indicates if the restaurant is operational.

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  // Use UTC for consistency in timestamps.
        public DateTime? UpdatedAt { get; set; }  // Can be set automatically in application logic.

        // Navigation properties
        public virtual ICollection<Menu> Menus { get; set; } = new List<Menu>();  // One-to-many relationship with Menu.
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();  // One-to-many relationship with Order.
        public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();  // One-to-many relationship with Rating.
    }
}

