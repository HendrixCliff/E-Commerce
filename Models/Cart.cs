using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerce.API.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
        
        public int? UserId { get; set; } 
    }
}
