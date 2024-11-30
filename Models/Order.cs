using QuickServeAPP.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace QuickServeAPP.Models
{
    public enum OrderStatus
    {
        Pending,
        Confirmed,
        InProgress,
        OutForDelivery, // Order is with the delivery person
        Delivered,
        Completed,
        Canceled
    }

    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [Required(ErrorMessage = "User ID is required.")]
        public int UserID { get; set; }  // Foreign Key for User

        [Required(ErrorMessage = "Restaurant ID is required.")]
        public int RestaurantID { get; set; }  // Foreign Key for Restaurant

        [Required(ErrorMessage = "Order status is required.")]
        [JsonConverter(typeof(JsonStringEnumConverter))] // Converts enum to string in responses
        public OrderStatus OrderStatus { get; set; }  // Enum for order status

        [Required(ErrorMessage = "Order date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;  // Default to UTC now if not provided

        [Required(ErrorMessage = "Total amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than zero.")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }  // Add Address

        // Navigation properties
        public virtual User User { get; set; }  // Many-to-one with User
        public virtual Restaurant Restaurant { get; set; }  // Many-to-one with Restaurant
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();  // One-to-many with OrderItem
        public virtual Payment Payment { get; set; }  // One-to-one with Payment
    }
}

