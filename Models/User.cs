using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace QuickServeAPP.Models
{
    //public enum UserRole
    //{
    //    Admin,
    //    Customer,
    //    RestaurantOwner,
    //    DeliveryPerson
    //}

    public enum Gender
    {
        Male,
        Female,
        Other,
        PreferNotToSay
    }

    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        public Gender? Gender { get; set; }

        [Required(ErrorMessage = "Contact number is required.")]
        [Phone(ErrorMessage = "Invalid contact number format.")]
        [StringLength(15, ErrorMessage = "Contact number cannot exceed 15 characters.")]
        public string ContactNumber { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters.")]
        public string Email { get; set; }

        // Hashed password storage
        [Required]
        public string PasswordHash { get; set; }

        // Enum for role
        [Required]
        public string Role { get; set; } = "Customer";

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
        public virtual Cart Cart { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();  // One-to-many with Payment


        public DateTime? DateOfBirth { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; }

    }
}