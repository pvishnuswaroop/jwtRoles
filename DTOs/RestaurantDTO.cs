
namespace QuickServeAPP.DTOs
{
    public class RestaurantDto
    {
        public int RestaurantID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
    public class UpdateRestaurantStatusDto
    {
        public bool IsActive { get; set; }
    }
    

}

