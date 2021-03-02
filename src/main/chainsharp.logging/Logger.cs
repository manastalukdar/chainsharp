// --------------------------------------------------------
// Copyright (c) 2017 - 2018 OSIsoft, LLC. All rights reserved.
//
// THIS SOFTWARE CONTAINS CONFIDENTIAL INFORMATION AND
// TRADE SECRETS OF OSIsoft, LLC. USE, DISCLOSURE, OR
// REPRODUCTION IS PROHIBITED WITHOUT THE PRIOR EXPRESS
// WRITTEN PERMISSION OF OSIsoft, LLC.
//
// RESTRICTED RIGHTS LEGEND
// Use, duplication, or disclosure by the Government is
// subject to restrictions as set forth in subparagraph
// (c)(1)(ii) of the Rights in Technical Data and
// Computer Software clause at DFARS 252.227.7013
//
// OSIsoft, LLC
// 1600 Alvarado Street, San Leandro, CA 94577
// --------------------------------------------------------
using Serilog.Events;

namespace chainsharp.logging
{
    public class Logger : ILoggingService
    {
        #region Private Fields

        private readonly LoggerBase _loggerBase;

        #endregion Private Fields

        #region Public Constructors

        public Logger(string location, string rollingFileName)
        {
            _loggerBase = new LoggerBase(location, rollingFileName);
            Location = location;
            RollingFileName = rollingFileName;
        }

        #endregion Public Constructors

        #region Public Properties

        public string Location { get; private set; }

        public string RollingFileName { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public void Log(ILogData data)
        {
            if (data is LogData logData)
            {
                Log(logData);
            }
            else
            {
                // If we get an unknown implementation, we log the bare minimmum.
                _loggerBase.LogInformation("This log entry originated from an unknown ILogData implementation", data.TenantId);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void Log(LogData data)
        {
            switch (data.LogSeverityLevel)
            {
                case LogEventLevel.Verbose:
                    _loggerBase.LogVerbose(data.Message, data.TenantId, data.Request);
                    break;

                case LogEventLevel.Warning:
                    _loggerBase.LogWarning(data.Message, data.TenantId, data.Request);
                    break;

                case LogEventLevel.Error:
                    _loggerBase.LogError(data.Message, data.TenantId, data.Request);
                    break;

                case LogEventLevel.Fatal:
                    _loggerBase.LogCritical(data.Message, data.TenantId, data.Request);
                    break;

                case LogEventLevel.Information:
                    _loggerBase.LogInformation(data.Message, data.TenantId, data.Request);
                    break;

                case LogEventLevel.Debug:
                    _loggerBase.LogDebug(data.Message, data.TenantId, data.Request);
                    break;

                default:
                    // Defaults to Information if we get an unknown enum
                    _loggerBase.LogInformation(data.Message, data.TenantId, data.Request);
                    break;
            }
        }

        #endregion Private Methods
    }
}
