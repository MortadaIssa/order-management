
namespace OrderManagement.Application.DTOs.Orders
{
    public class OrderItemDto
    {
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
