using SecurityDotnet.Dtos;

namespace SecurityDotnet.Service
{
    public interface IAuthService
    {
        Task<RegisterResponseDto?> RegisterAsync(RegisterDto request);
        Task<TokenResponseDto?> LoginAsync(LoginDto request);
        Task<TokenResponseDto?> AccessTokenAsync(AccessTokenRequestDto request);
        Task<object?> PasswordResetEmailAsync(string email);
    }
}
