using OrderManagement.Application.DTOs.Auth;
using OrderManagement.Domain.Entities;
using System.Threading.Tasks;

namespace OrderManagement.Application.Interfaces
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(RegisterUserDto dto);
        string CreateToken(User user);
    }
}
