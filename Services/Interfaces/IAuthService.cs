using Ecommerce.API.DTOs.Auth;

namespace Ecommerce.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto);
        Task<AuthResponseDto> LoginAsync(LoginRequestDto dto);
        Task<string> ForgotPasswordAsync(ForgotPasswordRequestDto dto);
        Task<bool> ResetPasswordAsync(ResetPasswordRequestDto dto);
    }
}
