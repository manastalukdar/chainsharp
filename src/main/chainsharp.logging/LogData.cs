using System.Runtime.Serialization;
using Serilog.Events;

namespace chainsharp.logging
{
    public class LogData : ILogData
    {
        #region Public Constructors

        public LogData()
        {
        }

        #endregion Public Constructors

        #region Public Properties

        [DataMember]
        public LogEventLevel LogSeverityLevel { get; set; }

        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public string OperationId { get; set; }

        [DataMember]
        public string Request { get; set; }

        [DataMember]
        public string TenantId { get; set; }

        #endregion Public Properties
    }
}
