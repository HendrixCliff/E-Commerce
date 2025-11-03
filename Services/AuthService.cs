using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using BCrypt.Net;
using Ecommerce.API.DTOs.Auth;
using Ecommerce.API.Models;
using Ecommerce.API.Repositories.Interfaces;
using Ecommerce.API.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Ecommerce.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _config;
        private readonly IEmailService _emailService;
        public AuthService(IUserRepository userRepository, IConfiguration config, IEmailService emailService)
        {
            _userRepository = userRepository;
            _config = config;
            _emailService = emailService;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto)
        {
            var existing = await _userRepository.GetByEmailAsync(dto.Email);
            if (existing != null)
                throw new Exception("User already exists");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            var newUser = new User
            {
                Email = dto.Email,
                PasswordHash = hashedPassword
            };

            await _userRepository.AddAsync(newUser);
            await _userRepository.SaveChangesAsync();

            var token = GenerateJwtToken(newUser);
            return new AuthResponseDto
            {
                Token = token,
                Email = newUser.Email
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid credentials");

            var token = GenerateJwtToken(user);
            return new AuthResponseDto { Token = token, Email = user.Email };
        }

        private string GenerateJwtToken(User user)
        {
            var jwtKey = _config["Jwt:Key"];
            var jwtIssuer = _config["Jwt:Issuer"];

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtIssuer,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


   public async Task ForgotPasswordAsync(ForgotPasswordRequestDto dto)
{
    var user = await _userRepository.GetByEmailAsync(dto.Email);
    if (user == null) return;

    var token = Guid.NewGuid().ToString();
    user.PasswordResetToken = token;
    user.ResetTokenExpiry = DateTime.UtcNow.AddHours(1);

    await _userRepository.UpdateAsync(user);
    await _userRepository.SaveChangesAsync();

    var resetLink = $"https://yourfrontend.com/reset-password?token={token}&email={dto.Email}";
    await _emailService.SendEmailAsync(dto.Email, "Reset Your Password",
        $"Click the link to reset your password: {resetLink}");
}



          public async Task<bool> ResetPasswordAsync(ResetPasswordRequestDto dto)
        {
            var user = (await _userRepository.GetAllAsync())
                .FirstOrDefault(u => u.PasswordResetToken == dto.Token && u.ResetTokenExpiry > DateTime.UtcNow);

            if (user == null)
                throw new Exception("Invalid or expired reset token");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            user.PasswordResetToken = null;
            user.ResetTokenExpiry = null;

            await _userRepository.SaveChangesAsync();
            return true;
        }
    }
}
