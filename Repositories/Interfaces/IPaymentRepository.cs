using Ecommerce.API.Models;

namespace Ecommerce.API.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<Payment>> GetAllAsync();
        Task<Payment?> GetByIdAsync(int id);
        Task AddAsync(Payment payment);
        Task SaveChangesAsync();
    }
}
