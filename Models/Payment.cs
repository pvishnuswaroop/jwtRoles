using QuickServeAPP.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuickServeAPP.Models
{
    public enum PaymentMethodEnum
    {
        CreditCard,
        DebitCard,
        PayPal,
        CashOnDelivery
    }

    // Optionally, you can define the PaymentStatusEnum to replace string-based status
    public enum PaymentStatusEnum
    {
        Pending,
        Completed,
        Failed,
        Refunded
    }

    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }

        [Required(ErrorMessage = "Order ID is required.")]
        [ForeignKey("Order")]
        public int OrderID { get; set; }  // Foreign Key for Order

        [Required(ErrorMessage = "Amount paid is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount paid must be greater than zero.")]
        public decimal AmountPaid { get; set; }

        [Required(ErrorMessage = "Payment status is required.")]
        public PaymentStatusEnum PaymentStatus { get; set; }  // Enum for payment status (e.g., "Completed", "Pending")

        [Required(ErrorMessage = "Payment date is required.")]
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;  // Default to UTC now if not provided

        [Required(ErrorMessage = "Payment method is required.")]
        public PaymentMethodEnum PaymentMethod { get; set; }  // Enum for payment method

        // Navigation properties
        public virtual Order Order { get; set; }  // One-to-one with Order
        public virtual User User { get; set; }  // Navigation property to User (based on your use case)

        // Optional: If you want to track the user making the payment, add a foreign key
        // If you already have a user relationship via Order, this field is unnecessary
        [Required(ErrorMessage = "User ID is required.")]
        [ForeignKey("User")]
        public int UserID { get; set; }  // Foreign Key for User
    }
}

