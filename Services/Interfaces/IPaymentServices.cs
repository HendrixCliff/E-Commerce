using Ecommerce.API.Models;

namespace Ecommerce.API.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<Payment> InitializePayment(decimal amount, string email, int orderId);
        Task<Payment?> VerifyPayment(string transactionId);
    }
}
