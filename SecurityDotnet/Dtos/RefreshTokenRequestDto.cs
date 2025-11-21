using System.ComponentModel.DataAnnotations;

namespace SecurityDotnet.Dtos
{
    public class RefreshTokenRequestDto
    {
        [Required(AllowEmptyStrings =false, ErrorMessage ="Valid user id is required")]
        public Guid UserId { get; set; }

        [Required(AllowEmptyStrings =false, ErrorMessage ="Refresh token is required")]
        public string RefreshToken { get; set; }
    }
}
