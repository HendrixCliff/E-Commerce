using Ecommerce.API.Models;
using Ecommerce.API.Repositories.Interfaces;
using Ecommerce.API.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Ecommerce.API.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IConfiguration _configuration;

        public PaymentService(HttpClient httpClient, IPaymentRepository paymentRepository, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _paymentRepository = paymentRepository;
            _configuration = configuration;
        }

        public async Task<Payment> InitializePayment(decimal amount, string email, int orderId)
        {
            var secretKey = _configuration["Flutterwave:SecretKey"];

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", secretKey);

            var requestData = new
            {
                tx_ref = Guid.NewGuid().ToString(),
                amount = amount,
                currency = "NGN",
                redirect_url = "https://your-frontend.com/payment-success",
                customer = new { email = email },
                payment_options = "card"
            };

            var json = JsonSerializer.Serialize(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.flutterwave.com/v3/payments", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Payment initialization failed: {responseContent}");

            var result = JsonSerializer.Deserialize<JsonElement>(responseContent);
            var paymentLink = result.GetProperty("data").GetProperty("link").GetString();
            var transactionId = result.GetProperty("data").GetProperty("tx_ref").GetString();

            var payment = new Payment
            {
                TransactionId = transactionId ?? Guid.NewGuid().ToString(),
                Amount = amount,
                Status = "initialized",
                Currency = "NGN",
                OrderId = orderId
            };

            await _paymentRepository.AddAsync(payment);
            await _paymentRepository.SaveChangesAsync();

            return payment;
        }

        public async Task<Payment?> VerifyPayment(string transactionId)
        {
            var secretKey = _configuration["Flutterwave:SecretKey"];

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", secretKey);

            var response = await _httpClient.GetAsync($"https://api.flutterwave.com/v3/transactions/{transactionId}/verify");
            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<JsonElement>(responseContent);
            var status = result.GetProperty("data").GetProperty("status").GetString();

            var payment = await _paymentRepository.GetByIdAsync(transactionId.GetHashCode());
            if (payment != null)
            {
                payment.Status = status ?? "unknown";
                await _paymentRepository.SaveChangesAsync();
            }

            return payment;
        }
    }
}
