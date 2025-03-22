using System;

namespace SalesManagement.Infrastructure.CrossCutting.Logging
{
    public interface ILogService
    {
        void LogInformation(string message);
        void LogWarning(string message);
        void LogError(string message);
        void LogError(Exception exception, string message);
    }
}