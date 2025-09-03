 using System.ComponentModel.DataAnnotations;

namespace EmployeeFrontendClient.Models.ViewModels
{
    public class RegisterViewModel
    {
        //[Required(ErrorMessage = "Username is required")]
        //[StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
        //[Display(Name = "Username")]
        //public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm password")]
        //[Compare("Password", ErrorMessage = "Password and confirmation password do not match")]
        //public string ConfirmPassword { get; set; } = string.Empty;
    }
}