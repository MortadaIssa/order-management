
namespace OrderManagement.Application.Interfaces
{
    public interface ILoggingService
    {
        void LogError(string message);
        void LogAudit(string message);
    }
}
