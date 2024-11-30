namespace QuickServeAPP.DTOs
{
    public class CartDto
    {
        public int CartID { get; set; }
        public int UserID { get; set; }
        public List<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();
        public DateTime CreationDate { get; set; }
        public bool IsActive { get; set; }
    }


    public class CartItemDto
    {
        public int MenuID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }  // Include price in DTO to show on the client side
        
    }
}
