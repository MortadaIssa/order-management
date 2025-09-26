using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OrderManagement.Application.DTOs.Auth;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<User> RegisterAsync(RegisterUserDto dto)
        {
            // Simple HMACSHA256-based password hashing with per-user salt (for demo only).
            var salt = GenerateSalt();
            var hashed = HashPassword(dto.Password, salt);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                Salt = Convert.ToBase64String(salt),
                PasswordHash = Convert.ToBase64String(hashed)
            };

            await _userRepository.AddAsync(user);
            return user;
        }

        public string CreateToken(User user)
        {
            var tokenSecret = _configuration["Jwt:Secret"] ?? throw new InvalidOperationException("JWT secret not configured.");
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSecret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("name", user.Name)
            };

            var token = new JwtSecurityToken(
                issuer,
                audience,
                claims,
                expires: DateTime.UtcNow.AddHours(6),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<User?> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null) return null;

            // retrieve salt and verify password using the same HMACSHA256 approach
            var salt = Convert.FromBase64String(user.Salt);
            using var hmac = new HMACSHA256(salt);
            var hashed = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hashedBase64 = Convert.ToBase64String(hashed);

            if (hashedBase64 != user.PasswordHash)
            {
                return null; // invalid credentials
            }

            return user;
        }

        private static byte[] GenerateSalt()
        {
            using var rng = RandomNumberGenerator.Create();
            var salt = new byte[16];
            rng.GetBytes(salt);
            return salt;
        }

        private static byte[] HashPassword(string password, byte[] salt)
        {
            using var hmac = new HMACSHA256(salt);
            return hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}
