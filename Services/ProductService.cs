using Ecommerce.API.Models;
using Ecommerce.API.Repositories.Interfaces;
using Ecommerce.API.Services.Interfaces;

namespace Ecommerce.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Product> CreateAsync(Product product)
        {
            await _repository.AddAsync(product);
            await _repository.SaveChangesAsync();
            return product;
        }

        public async Task<bool> UpdateAsync(int id, Product updatedProduct)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null)
                return false;

            existing.Name = updatedProduct.Name;
            existing.Description = updatedProduct.Description;
            existing.Price = updatedProduct.Price;
            existing.Stock = updatedProduct.Stock;

            await _repository.UpdateAsync(existing);
            await _repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
                return false;

            await _repository.DeleteAsync(product);
            await _repository.SaveChangesAsync();

            return true;
        }
    }
}
