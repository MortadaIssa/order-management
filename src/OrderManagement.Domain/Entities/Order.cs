
namespace OrderManagement.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public decimal TotalPrice { get; set; }
        public List<OrderItem> Items { get; set; } = new();
    }
}
