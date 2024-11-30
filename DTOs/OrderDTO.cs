using QuickServeAPP.Models;
using System.ComponentModel.DataAnnotations;

namespace QuickServeAPP.DTOs
{
    public class OrderDto
    {
        public int OrderID { get; set; }
        public int UserID { get; set; }
        public int RestaurantID { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
        public string Address { get; set; }
        public decimal TotalAmount { get; set; } // This will be calculated by the server
        public string OrderStatus { get; set; } // Default status
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    }

    public class InitializeOrderDto
    {
        [Required]
        public string Address { get; set; } // Delivery address
    }

}
