using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using MimeKit.Text;
using MimeKit.Utils;
using SecurityDotnet.Data;
using SecurityDotnet.Dtos;
using SecurityDotnet.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SecurityDotnet.Service
{
    public class AuthService(IConfiguration configuration, AppDbContext appDbContext) : IAuthService
    {
        public async Task<TokenResponseDto?> LoginAsync(LoginDto request)
        {
            User? user = await appDbContext.Users.FirstOrDefaultAsync(user => user.Email == request.Email);
            if (user == null) return null;

            bool incorrectPassword = new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, request.Password)
                == PasswordVerificationResult.Failed;
            if (incorrectPassword) return null;

            string refreshToken = await GenerateAndSaveRefreshToken(user);
            string accessToken = CreateAccessToken(user);

            return new TokenResponseDto { AccessToken = accessToken, RefreshToken = refreshToken, UserId = user.Id };
        }

        public async Task<RegisterResponseDto?> RegisterAsync(RegisterDto request)
        {
            bool userExist = await appDbContext.Users.AnyAsync(user => user.Email == request.Email);
            if (userExist) return null;

            User user = new() { Role = request.Role };
            string hashedPassword = new PasswordHasher<User>()
                .HashPassword(user, request.Password);

            user.Email = request.Email;
            user.Password = hashedPassword;

            await appDbContext.Users.AddAsync(user);
            await appDbContext.SaveChangesAsync();

            string accessToken = CreateAccessToken(user);
            string refreshToken = await GenerateAndSaveRefreshToken(user);

            return new RegisterResponseDto {
                message = "Registered successfully",
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserId = user.Id };
        }

        private string CreateAccessToken(User user)
        {
            List<Claim> claims =
            [
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            ];

            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AppSettings:Token"]!));

            SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken securityToken = new JwtSecurityToken(
                issuer: configuration["AppSettings:Issuer"],
                audience: configuration["AppSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        private static string CreateRefreshToken()
        {
            byte[] randomNumber = new byte[32];
            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task<string> GenerateAndSaveRefreshToken(User user)
        {
            string refreshToken = CreateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

            await appDbContext.SaveChangesAsync();
            return refreshToken;
        }

        public async Task<TokenResponseDto?> AccessTokenAsync(AccessTokenRequestDto request)
        {
            User? user = await appDbContext.Users.FindAsync(request.UserId);
            if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow) 
                return null;

            string accessToken = CreateAccessToken(user);
            return new TokenResponseDto() { AccessToken = accessToken, RefreshToken = request.RefreshToken, UserId = user.Id };
        }

        public async Task<object?> PasswordResetEmailAsync(string email)
        {
            User? user = await appDbContext.Users.FirstOrDefaultAsync(user => user.Email == email);
            if (user == null) return null;

            string code = GeneratePasswordResetCode();
            user.PasswordResetCode = code;
            user.PasswordResetCodeTime = DateTime.UtcNow.AddMinutes(10);
            appDbContext.SaveChanges();

            await SendPasswordResetEmail(email, code);

            return new { message = "Password reset email sent successfully" };
        }

        private static string GeneratePasswordResetCode()
        {
            Random random = new Random();
            StringBuilder code = new StringBuilder();

            for (int i = 0; i < 6; i++)
            {
                code.Append(random.Next(0, 10));
            }

            return code.ToString();
        }

        private async Task SendPasswordResetEmail(string email, string code)
        {
            MimeMessage message = new MimeMessage();

            MailboxAddress fromAddress = new MailboxAddress("From", "yibavo8795@bipochub.com");
            message.From.Add(fromAddress);
            MailboxAddress toAddress = new MailboxAddress("To", email);
            message.To.Add(toAddress);

            message.Subject = "Password reset email";
            //message.Body = new TextPart(TextFormat.Html)
            //{
            //    Text = $"""
            //    Bellow is the password reset code

            //    {code}
            //    """
            //};

            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = $"Password reset code: {code}";

            MimeEntity mimeEntity = bodyBuilder.LinkedResources.Add("cat.jpg");
            mimeEntity.ContentId = MimeUtils.GenerateMessageId();
            var htmlBody = $"""
                <p>Hello, below is the password reset code</p>
                {code}

                <img src="cid:{mimeEntity.ContentId}" alt="Ginger cat!" />
                """;

            bodyBuilder.HtmlBody = htmlBody;
            //bodyBuilder.Attachments.Add("cat.jpg");
            message.Body = bodyBuilder.ToMessageBody();

            SmtpClient smtpClient = new SmtpClient();
            await smtpClient.ConnectAsync("localhost", 1025);
            await smtpClient.SendAsync(message);
            await smtpClient.DisconnectAsync(true);
        }

        public async Task<ResponseDto?> ResetPasswordAsync(PasswordResetDto request)
        {
            User? user = await appDbContext.Users.FirstOrDefaultAsync(user => user.PasswordResetCode == request.Code);
            if (user == null) return null;
            if (user.PasswordResetCodeTime != null && user.PasswordResetCodeTime <= DateTime.UtcNow) return null;

            string hassedPassword = new PasswordHasher<User>()
                .HashPassword(user, request.Password);
            user.Password = hassedPassword;
            user.PasswordResetCodeTime = DateTime.UtcNow;

            await appDbContext.SaveChangesAsync();

            return new ResponseDto { Message = "Password reset successful" };
        }
    }
}
