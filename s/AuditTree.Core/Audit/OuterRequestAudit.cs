using System;
using AuditTree.Core.AuditLogger;

namespace AuditTree.Core.Audit
{
    public class OuterRequestAudit
    {
        internal string RequestId => Guid.NewGuid().ToString();

        private readonly IRequestLogger _requestLogger;
        private readonly Lazy<LoggerData> _loggerData;

        public OuterRequestAudit(IRequestLogger requestLogger, Lazy<LoggerData> loggerData)
        {
            _requestLogger = requestLogger;
            _loggerData = loggerData;
        }


        public Exception LogError(Exception e, string msg)
        {
            return _requestLogger.LogRequestError(RequestId, _loggerData.Value, e, msg);
        }

        public void LogResponce(string data)
        {
            _requestLogger.LogResponce(RequestId, _loggerData.Value, data);
        }

        public void LogRequest(string data)
        {
            _requestLogger.LogRequestData(RequestId, _loggerData.Value, data);
        }
    }
}
