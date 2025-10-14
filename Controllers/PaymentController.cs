using Ecommerce.API.Models;
using Ecommerce.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost("initialize")]
        public async Task<IActionResult> InitializePayment([FromBody] PaymentRequest request)
        {
            var payment = await _paymentService.InitializePayment(request.Amount, request.Email, request.OrderId);
            return Ok(payment);
        }

        [HttpGet("verify/{transactionId}")]
        public async Task<IActionResult> VerifyPayment(string transactionId)
        {
            var payment = await _paymentService.VerifyPayment(transactionId);
            if (payment == null) return NotFound();

            return Ok(payment);
        }
    }

    public class PaymentRequest
    {
        public decimal Amount { get; set; }
        public string Email { get; set; } = string.Empty;
        public int OrderId { get; set; }
    }
}
