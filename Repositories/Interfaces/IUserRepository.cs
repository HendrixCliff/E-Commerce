using Ecommerce.API.Models;

namespace Ecommerce.API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int id);
        Task<IEnumerable<User>> GetAllAsync();
        Task UpdateAsync(User user);
        Task AddAsync(User user);
        Task SaveChangesAsync();
    }
}
