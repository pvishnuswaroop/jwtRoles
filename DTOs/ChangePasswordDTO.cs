﻿using System.ComponentModel.DataAnnotations;
namespace QuickServeAPP.DTOs
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Current password is required.")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "New password is required.")]
        [MinLength(6, ErrorMessage = "New password must be at least 6 characters long.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm password is required.")]
        [Compare("NewPassword", ErrorMessage = "Passwords must match.")]
        public string ConfirmPassword { get; set; }
    }
}