using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OrderManagement.Application.DTOs.Auth;
using OrderManagement.Application.Features.Users;
using OrderManagement.Application.Interfaces;
using OrderManagement.Application.Services;
using System.Threading.Tasks;

namespace OrderManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly RegisterUserHandler _registerHandler;
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(RegisterUserHandler registerHandler, IAuthService authService, ILogger<AuthController> logger)
        {
            _registerHandler = registerHandler;
            _authService = authService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
        {
            // FluentValidation will run automatically via middleware configured in Program.cs
            var (user, token) = await _registerHandler.HandleAsync(dto);
            _logger.LogInformation("User registered: {UserId}", user.Id);

            return Created(string.Empty, new { user.Id, token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _authService.LoginAsync(dto.Email, dto.Password);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            var token = _authService.CreateToken(user);
            _logger.LogInformation("User logged in: {UserId}", user.Id);

            return Ok(new { user.Id, token });
        }
    }
}
