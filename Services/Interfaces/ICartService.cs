using Ecommerce.API.Models;

namespace Ecommerce.API.Services.Interfaces
{
    public interface ICartService
    {
        Task<IEnumerable<Cart>> GetAllAsync();
        Task<Cart?> GetByIdAsync(int id);
        Task<Cart> CreateAsync(Cart cart);
        Task<bool> UpdateAsync(int id, Cart updatedCart);
        Task<bool> DeleteAsync(int id);
    }
}
