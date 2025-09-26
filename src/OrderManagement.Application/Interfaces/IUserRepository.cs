using OrderManagement.Domain.Entities;


namespace OrderManagement.Application.Interfaces
{
    public interface IUserRepository : Domain.Interfaces.IRepository<User>
    {
        // Add user-specific queries here if needed
        Task<User?> GetByEmailAsync(string email);
    }
}
