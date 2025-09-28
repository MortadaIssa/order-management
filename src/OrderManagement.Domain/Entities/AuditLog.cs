using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Domain.Entities
{
    public class AuditLog
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Level { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string Message { get; set; } = null!;
        public string? Exception { get; set; }
        public string? UserId { get; set; }
        public string? Data { get; set; } // optional JSON
    }
}
