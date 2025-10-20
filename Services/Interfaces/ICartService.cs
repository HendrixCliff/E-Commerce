using Ecommerce.API.DTOs.Cart;
using Ecommerce.API.Models;

namespace Ecommerce.API.Services.Interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<CartResponseDto>> GetAllAsync();
        Task<CartResponseDto?> GetByIdAsync(int id);
        Task<CartResponseDto> CreateAsync(CreateCartDto dto);
        Task<bool> UpdateAsync(int id, UpdateCartDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
