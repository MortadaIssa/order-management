using OrderManagement.Application.DTOs;

namespace OrderManagement.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> RegisterAsync(RegisterUserDto dto, CancellationToken cancellationToken = default);
        Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
    }
}
