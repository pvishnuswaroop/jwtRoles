namespace QuickServeAPP.DTOs
{
    public class OrderItemDto
    {
        public int MenuID { get; set; }
        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
