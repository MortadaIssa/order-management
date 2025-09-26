using AutoMapper;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.Infrastructure.Data;

namespace OrderManagement.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public UserService(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<UserDto> RegisterAsync(RegisterUserDto dto, CancellationToken cancellationToken = default)
        {
            var exists = await _db.Users.AnyAsync(u => u.Email == dto.Email, cancellationToken);
            if (exists) throw new InvalidOperationException("Email already in use.");

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _db.Users.Add(user);

            // add audit
            var audit = new AuditLog
            {
                Entity = nameof(User),
                Action = "Create",
                Details = $"User created: {user.Email}",
                PerformedBy = user.Email
            };
            _db.AuditLogs.Add(audit);

            await _db.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UserDto>(user);
        }

        public Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default)
        {
            return _db.Users.AnyAsync(u => u.Email == email, cancellationToken);
        }
    }
}
