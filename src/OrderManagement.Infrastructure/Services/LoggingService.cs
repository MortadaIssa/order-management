using OrderManagement.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Infrastructure.Services
{
    public class LoggingService : ILoggingService
    {
        public void LogError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERROR] {DateTime.UtcNow:u} - {message}");
            Console.ResetColor();
        }

        public void LogAudit(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[AUDIT] {DateTime.UtcNow:u} - {message}");
            Console.ResetColor();
        }
    }
}
