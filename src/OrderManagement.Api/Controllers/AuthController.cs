using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Interfaces;

namespace OrderManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserService userService, ILogger<AuthController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto, CancellationToken cancellationToken)
        {
            if (dto == null) return BadRequest("Payload is required.");

            // minimal validation - you will add FluentValidation later
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                return BadRequest("Email and password are required.");

            try
            {
                var user = await _userService.RegisterAsync(dto, cancellationToken);
                return CreatedAtAction(null, new { id = user.Id }, user);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Registration conflict for {Email}", dto.Email);
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during registration.");
                return StatusCode(500, new { message = "An error occurred." });
            }
        }
    }
}
