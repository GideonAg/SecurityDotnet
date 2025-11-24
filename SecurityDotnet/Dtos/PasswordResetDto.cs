using System.ComponentModel.DataAnnotations;

namespace SecurityDotnet.Dtos
{
    public class PasswordResetDto
    {
        [Required(AllowEmptyStrings =false, ErrorMessage ="Password is required")]
        [StringLength(maximumLength: 12, MinimumLength = 6, ErrorMessage ="Password should be between 6 to 12 characters")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password reset code is required")]
        [StringLength(maximumLength:6, MinimumLength =6, ErrorMessage ="Valid password reset code is required")]
        public string Code { get; set; }
    }
}
