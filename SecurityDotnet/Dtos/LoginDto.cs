using System.ComponentModel.DataAnnotations;

namespace SecurityDotnet.Dtos
{
    public class LoginDto
    {
        [EmailAddress(ErrorMessage = "Valid email is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
