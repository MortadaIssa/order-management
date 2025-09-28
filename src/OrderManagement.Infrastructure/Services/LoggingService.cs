using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.Infrastructure.Data;
using OrderManagement.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Infrastructure.Services
{
    public class LoggingService : BaseRepository<AuditLog>, ILoggingService
    {
        public LoggingService(AppDbContext context) : base(context)
        {
        }

        public async Task LogInfoAsync(string category, string message, string? userId = null, string? data = null)
        {
            var log = new AuditLog
            {
                Level = "Info",
                Category = category,
                Message = message,
                UserId = userId,
                Data = data
            };

            await AddAsync(log); // uses BaseRepository.AddAsync -> SaveChangesAsync()
        }

        public async Task LogErrorAsync(string category, string message, Exception? ex = null, string? userId = null, string? data = null)
        {
            var log = new AuditLog
            {
                Level = "Error",
                Category = category,
                Message = message,
                Exception = ex?.ToString(),
                UserId = userId,
                Data = data
            };

            await AddAsync(log);
        }
    }
}
