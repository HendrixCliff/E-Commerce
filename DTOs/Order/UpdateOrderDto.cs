namespace Ecommerce.API.DTOs.Order
{
    public class UpdateOrderDto
    {
        public int Id { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
