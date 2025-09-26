using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using OrderManagement.Application.DTOs.Auth;
using OrderManagement.Application.Features.Users;
using OrderManagement.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace OrderManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly RegisterUserHandler _registerHandler;
        private readonly ILogger<AuthController> _logger;

        public AuthController(RegisterUserHandler registerHandler, ILogger<AuthController> logger)
        {
            _registerHandler = registerHandler;
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
    }
}
