using System.ComponentModel.DataAnnotations;

namespace QuickServeAPP.DTOs
{
    public class RatingDto
    {
        public int RatingID { get; set; }
        public int UserID { get; set; }
        public int RestaurantID { get; set; }
        public int? MenuID { get; set; }
        public int? OrderID { get; set; }
        public int RatingScore { get; set; }
        public string ReviewText { get; set; }
        public DateTime RatingDate { get; set; }
    }

    public class SubmitRatingDto
    {
        [Required]
        public int UserID { get; set; }

        [Required]
        public int RestaurantID { get; set; }

        [Required]
        public int? OrderID { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int RatingScore { get; set; }

        [StringLength(500)]
        public string ReviewText { get; set; }
    }
}
