using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SecurityDotnet.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        //[Required(AllowEmptyStrings =false, ErrorMessage ="First name is required")]
        //public string FirstName { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Last name is required")]
        //public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public required string Role { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime {  get; set; }
        public string? PasswordResetCode { get; set; }
    }
}
