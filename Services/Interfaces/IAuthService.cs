using Ecommerce.API.Models;

namespace Ecommerce.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(string email, string password);
        Task<string> LoginAsync(string email, string password);
    }
}
