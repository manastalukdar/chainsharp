using Serilog;
using Serilog.Core;
using System;
using System.IO;
using System.Text;

namespace chainsharp.logging
{
    public class LoggerBase
    {
        #region Private Fields

        private readonly ILogger _logger;

        #endregion Private Fields

        #region Public Constructors

        public LoggerBase(string location, string rollingFileName)
        {
            LevelSwitch = new LoggingLevelSwitch
            {
                MinimumLevel = Serilog.Events.LogEventLevel.Debug,
            };
            _logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(LevelSwitch)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File($"{location}{Path.DirectorySeparatorChar}{rollingFileName}" + "-{DateTime.Today.Date.ToString()}.txt", rollOnFileSizeLimit: true)
                .CreateLogger();
        }

        #endregion Public Constructors

        #region Public Properties

        public LoggingLevelSwitch LevelSwitch { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void LogCritical(string message, string tenantId = "", string namespaceId = "", string request = "", string operationId = "")
        {
            var stringCsv = LoggingUtils.ArrayToCsv(message, tenantId, namespaceId, request, operationId);
            _logger.Fatal(stringCsv);
        }

        public void LogCritical(Exception ex, string tenantId = "", string namespaceId = "", string request = "", string operationId = "")
        {
            var message = MessageFromException(ex);
            var stringCsv = LoggingUtils.ArrayToCsv(message, tenantId, namespaceId, request, operationId);
            _logger.Fatal(stringCsv);
        }

        public void LogDebug(string message, string tenantId = "", string namespaceId = "", string request = "", string operationId = "")
        {
            var stringCsv = LoggingUtils.ArrayToCsv(message, tenantId, namespaceId, request, operationId);
            _logger.Debug(stringCsv);
        }

        public void LogDebug(Exception ex, string tenantId = "", string namespaceId = "", string request = "", string operationId = "")
        {
            var message = MessageFromException(ex);
            var stringCsv = LoggingUtils.ArrayToCsv(message, tenantId, namespaceId, request, operationId);
            _logger.Debug(stringCsv);
        }

        public void LogError(string message, string tenantId = "", string namespaceId = "", string request = "", string operationId = "")
        {
            var stringCsv = LoggingUtils.ArrayToCsv(message, tenantId, namespaceId, request, operationId);
            _logger.Error(stringCsv);
        }

        public void LogError(Exception ex, string tenantId = "", string namespaceId = "", string request = "", string operationId = "")
        {
            var message = MessageFromException(ex);
            var stringCsv = LoggingUtils.ArrayToCsv(message, tenantId, namespaceId, request, operationId);
            _logger.Error(stringCsv);
        }

        public void LogInformation(string message, string tenantId = "", string namespaceId = "", string request = "", string operationId = "")
        {
            var stringCsv = LoggingUtils.ArrayToCsv(message, tenantId, namespaceId, request, operationId);
            _logger.Information(stringCsv);
        }

        public void LogVerbose(string message, string tenantId = "", string namespaceId = "", string request = "", string operationId = "")
        {
            var stringCsv = LoggingUtils.ArrayToCsv(message, tenantId, namespaceId, request, operationId);
            _logger.Verbose(stringCsv);
        }

        public void LogWarning(string message, string tenantId = "", string namespaceId = "", string request = "", string operationId = "")
        {
            var stringCsv = LoggingUtils.ArrayToCsv(message, tenantId, namespaceId, request, operationId);
            _logger.Warning(stringCsv);
        }

        public void LogWarning(Exception ex, string tenantId = "", string namespaceId = "", string request = "", string operationId = "")
        {
            var message = MessageFromException(ex);
            var stringCsv = LoggingUtils.ArrayToCsv(message, tenantId, namespaceId, request, operationId);
            _logger.Warning(stringCsv);
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Gets an object array with the parameters. The parameters are all passed as strings.
        /// </summary>
        private static string MessageFromException(Exception ex)
        {
            if (!(ex is AggregateException))
            {
                return ex.ToString();
            }

            // Aggregate exceptions only
            var aggregateException = (AggregateException)ex;
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("List of Inner Exceptions:").Append(Environment.NewLine);
            foreach (var innerException in aggregateException.Flatten().InnerExceptions)
            {
                stringBuilder.Append($"{innerException}").Append(Environment.NewLine);
            }

            return stringBuilder.ToString();
        }

        #endregion Private Methods
    }
}
