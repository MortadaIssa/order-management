using System;
using System.Collections.Generic;

namespace OrderManagement.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Salt { get; set; } = null!;

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
