
namespace OrderManagement.Domain.Entities
{
    public class AuditLog
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Entity { get; set; } = default!;
        public string Action { get; set; } = default!;
        public string Details { get; set; } = default!;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string PerformedBy { get; set; } = default!; // e.g., user email or system
    }
}
