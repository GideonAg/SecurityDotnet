using System.ComponentModel.DataAnnotations;

namespace SecurityDotnet.Dtos
{
    public class PasswordResetEmail
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
        public string Email { get; set; }
    }
}
