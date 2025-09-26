using OrderManagement.Application.DTOs.Auth;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using System.Threading.Tasks;

namespace OrderManagement.Application.Features.Users
{
    public class RegisterUserHandler
    {
        private readonly IAuthService _authService;

        public RegisterUserHandler(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<(User user, string token)> HandleAsync(RegisterUserDto dto)
        {
            var user = await _authService.RegisterAsync(dto);
            var token = _authService.CreateToken(user);
            return (user, token);
        }
    }
}
