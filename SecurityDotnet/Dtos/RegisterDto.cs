using System.ComponentModel.DataAnnotations;

namespace SecurityDotnet.Dtos
{
    public class RegisterDto
    {
        //[Required(AllowEmptyStrings = false, ErrorMessage = "First name is required")]
        //public string FirstName { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Last name is required")]
        //public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Valid email is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Role is required")]
        public string Role { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [StringLength(20, ErrorMessage = "Passwrod must be between 8 and 20 characters")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [StringLength(20, ErrorMessage = "Passwrod must be between 8 and 20 characters")]
        [Compare(nameof(Password), ErrorMessage ="Passwords do not match")]
        public string ConfirmPassword { get; set; }

    }
}
