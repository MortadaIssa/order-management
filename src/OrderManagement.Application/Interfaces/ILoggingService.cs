
namespace OrderManagement.Application.Interfaces
{
    public interface ILoggingService
    {
        Task LogInfoAsync(string category, string message, string? userId = null, string? data = null);
        Task LogErrorAsync(string category, string message, Exception? ex = null, string? userId = null, string? data = null);
    }
}
