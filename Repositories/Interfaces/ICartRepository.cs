using Ecommerce.API.Models;

namespace Ecommerce.API.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Task<IEnumerable<Cart>> GetAllAsync();
        Task<Cart?> GetByIdAsync(int id);
        Task AddAsync(Cart cart);
        Task UpdateAsync(Cart cart);
        Task DeleteAsync(Cart cart);
        Task SaveChangesAsync();
    }
}
