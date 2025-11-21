namespace SecurityDotnet.Dtos
{
    public class RegisterResponseDto
    {
        public required string message { get; set; }
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public Guid UserId { get; set; }
    }
}
