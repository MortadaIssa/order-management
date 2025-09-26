
namespace OrderManagement.Application.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
    }
}
