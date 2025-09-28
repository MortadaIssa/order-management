using OrderManagement.Application.DTOs.Auth;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using System.Threading.Tasks;

namespace OrderManagement.Application.Features.Users
{
    public class RegisterUserHandler
    {
        private readonly IAuthService _authService;
        private readonly ILoggingService _loggingService;

        public RegisterUserHandler(IAuthService authService, ILoggingService loggingService)
        {
            _authService = authService;
            _loggingService = loggingService;
        }

        public async Task<(User user, string token)> HandleAsync(RegisterUserDto dto)
        {
            var user = await _authService.RegisterAsync(dto);
            var token = _authService.CreateToken(user);

            await _loggingService.LogInfoAsync("Audit", $"User registered: {user.Id}");

            return (user, token);
        }
    }
}
