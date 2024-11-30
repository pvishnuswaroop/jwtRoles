using QuickServeAPP.Models;
namespace QuickServeAPP.DTOs
{
    public class MenuDto
    {
        public int MenuID { get; set; }
        public int RestaurantID { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string AvailabilityTime { get; set; }
        public string DietaryInfo { get; set; }
        public string Status { get; set; } // No change needed
    }


    public class MenuWithRatingDto
    {
        public int MenuID { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string AvailabilityTime { get; set; }
        public string DietaryInfo { get; set; }
        public string Status { get; set; }
        public double AverageRating { get; set; }
    }
}