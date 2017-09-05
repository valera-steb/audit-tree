using System;
using AuditTree.Core.AuditLogger;

namespace AuditTree.Core.Audit
{
    public class Audit
    {
        internal string Layer { get; }
        internal int LayerNum { get; }

        internal Lazy<LoggerData> LoggerData { get; }

        private readonly IAuditLogger _auditLogger;
        private readonly IRequestLogger _requestLogger;

        internal Audit(string layer, int layerNum, Func<LoggerData> getData, IAuditLogger auditLogger, IRequestLogger requestLogger)
        {
            Layer = layer;
            LayerNum = layerNum;
            _auditLogger = auditLogger;
            _requestLogger = requestLogger;
            LoggerData = new Lazy<LoggerData>(getData);
        }


        public OuterRequestAudit GetRequestAuditor()
        {
            var requestAudit = new OuterRequestAudit(_requestLogger, LoggerData);

            _auditLogger.Log(
                LoggerData.Value,
                $"start request ({requestAudit.RequestId})",
                null);

            return requestAudit;
        }

        public void LogInfo(string messatge, string data = null)
        {
            _auditLogger.Log(LoggerData.Value, messatge, data);
        }

        public Exception LogError(Exception e, string message = null)
        {
            return _auditLogger.LogError(LoggerData.Value, e, message);
        }
    }
}
