namespace Ecommerce.API.DTOs.Cart
{
    public class CreateCartDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int? UserId { get; set; }
    }
}