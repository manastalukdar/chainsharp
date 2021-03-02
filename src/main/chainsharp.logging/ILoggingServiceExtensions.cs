using System.Runtime.CompilerServices;
using Serilog.Events;

namespace chainsharp.logging
{
    public static class ILoggingServiceExtensions
    {
        #region Public Methods

        public static void Log(this ILoggingService service, string message, LogEventLevel level = LogEventLevel.Information, [CallerMemberName] string callerName = "", string operationId = "")
        {
            service.Log(new LogData()
            {
                Message = message,
                OperationId = operationId,
                LogSeverityLevel = level,
                Request = callerName,
            });
        }

        public static void LogCritical(this ILoggingService service, string message, [CallerMemberName] string callerName = "", string operationId = "")
        {
            service.Log(new LogData()
            {
                Message = message,
                OperationId = operationId,
                LogSeverityLevel = LogEventLevel.Fatal,
                Request = callerName,
            });
        }

        public static void LogError(this ILoggingService service, string message, [CallerMemberName] string callerName = "", string operationId = "")
        {
            service.Log(new LogData()
            {
                Message = message,
                OperationId = operationId,
                LogSeverityLevel = LogEventLevel.Error,
                Request = callerName,
            });
        }

        public static void LogWarning(this ILoggingService service, string message, [CallerMemberName] string callerName = "", string operationId = "")
        {
            service.Log(new LogData()
            {
                Message = message,
                OperationId = operationId,
                LogSeverityLevel = LogEventLevel.Warning,
                Request = callerName,
            });
        }

        #endregion Public Methods
    }
}
