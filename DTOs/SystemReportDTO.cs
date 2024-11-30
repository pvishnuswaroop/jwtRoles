namespace QuickServeAPP.DTOs
{
    public class SystemReportDto
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int TotalRestaurants { get; set; }
        public int ActiveRestaurants { get; set; }
        public int SuspendedRestaurants { get; set; }
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int CompletedOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public double AverageRating { get; set; }
    }
}
