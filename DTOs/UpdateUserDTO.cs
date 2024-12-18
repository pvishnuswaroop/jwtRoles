using System.ComponentModel.DataAnnotations;

namespace QuickServeAPP.DTOs
{
    public class UpdateUserDto
    {
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters.")]
        public string Address { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(15, ErrorMessage = "Phone number cannot exceed 15 characters.")]
        public string ContactNumber { get; set; }


        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }
    }

}