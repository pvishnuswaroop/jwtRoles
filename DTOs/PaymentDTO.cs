using QuickServeAPP.Models;
using System.ComponentModel.DataAnnotations;

namespace QuickServeAPP.DTOs
{
    public class PaymentDto
    {
        public int PaymentID { get; set; }
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public decimal AmountPaid { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; }
    }


    public class ProcessPaymentDto
    {
        [Required]
        public int OrderID { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal AmountPaid { get; set; }

        [Required]
        public PaymentMethodEnum PaymentMethod { get; set; }
    }
}
