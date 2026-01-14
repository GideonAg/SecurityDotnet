using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecurityDotnet.Dtos;
using SecurityDotnet.Service;

namespace SecurityDotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {

        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponseDto>> Register([FromBody] RegisterDto request)
        { 
            if (!ModelState.IsValid) return BadRequest(ModelState);

            RegisterResponseDto? result = await authService.RegisterAsync(request);
            if (result == null) return BadRequest(new { message = "User with this email already exist"});

            return Ok(result);
        }
         
        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login([FromBody] LoginDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            TokenResponseDto? result = await authService.LoginAsync(request);
            if (result == null) return Unauthorized(new {message = "Incorrect username or password"});

            return Ok(result);
        }

        [HttpGet("access-token")]
        public async Task<ActionResult<TokenResponseDto>> AccessToken([FromBody] AccessTokenRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            TokenResponseDto? tokenResponseDto = await authService.AccessTokenAsync(request);
            if (tokenResponseDto == null) return BadRequest(new { message = "Invalid refresh token or user id" });

            return Ok(tokenResponseDto);
        }

        [HttpGet("password-reset-email")]
        public async Task<object> PasswordResetEmail([FromBody] PasswordResetEmail request)
        {
            object? response = await authService.PasswordResetEmailAsync(request.Email);
            if (response == null) return BadRequest(new { message = "No user found with email" });
            return Ok(response);
        }

        [HttpPost("password-reset")]
        public async Task<ActionResult<ResponseDto>> PasswordReset([FromBody] PasswordResetDto request)
        {
            ResponseDto? responseDto = await authService.ResetPasswordAsync(request);
            if (responseDto == null) return BadRequest(new { message = "Invalid password reset code" } );

            return Ok(responseDto);
        }

        [HttpGet("authenticated")]
        [Authorize]
        public ActionResult<string> GetAuthenticatedData()
        {
            return Ok(new {message = "This is a protected api" });
        }

        [HttpGet("roles-required")]
        [Authorize(Roles = "User,Admin,Manager,CEO")]
        public ActionResult<string> RolesRequired()
        {
            return Ok(new { message = "Roles required to access" });
        }
    }
}
