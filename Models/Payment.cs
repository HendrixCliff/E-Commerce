using System.ComponentModel.DataAnnotations;

namespace Ecommerce.API.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        public string TransactionId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "NGN";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

       
        public int? OrderId { get; set; }
        public int? UserId { get; set; }
    }
}
