using Ecommerce.API.Models;
using Ecommerce.API.Repositories.Interfaces;

namespace Ecommerce.API.Services
{
    public class CartService
    {
        private readonly ICartRepository _repository;

        public CartService(ICartRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Cart>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Cart?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Cart> CreateAsync(Cart cart)
        {
            await _repository.AddAsync(cart);
            await _repository.SaveChangesAsync();
            return cart;
        }

        public async Task<bool> UpdateAsync(int id, Cart updatedCart)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return false;

            existing.ProductId = updatedCart.ProductId;
            existing.Quantity = updatedCart.Quantity;

            await _repository.UpdateAsync(existing);
            await _repository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var cart = await _repository.GetByIdAsync(id);
            if (cart == null)
                return false;

            await _repository.DeleteAsync(cart);
            await _repository.SaveChangesAsync();
            return true;
        }
    }
}
