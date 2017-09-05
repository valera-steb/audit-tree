using System;
using System.Text;
using AuditTree.Core.AuditLogger;

namespace Spec.Stubs
{
    public class StringAuditLogger : IAuditLogger, IRequestLogger
    {
        private readonly StringBuilder _sb;

        public StringAuditLogger(StringBuilder sb)
        {
            _sb = sb;
        }

        #region IAuditLogger
        public void Log(LoggerData data, string logMessage, string logData)
        {
            if (data.ParentLayerId != null)
                _sb.Append($"{data.ParentLayerId}_");

            _sb.AppendLine($"{data.LayerId} - {data.RequestId}.{data.SessionId}.{data.UserId} - {logMessage}");
        }

        public void LogInfo(LoggerData data, string msg)
        {
            _sb.AppendLine("i");
        }

        public Exception LogError(LoggerData data, Exception e, string msg = null)
        {
            _sb.AppendLine("e");
            return e;
        }
        #endregion


        #region IRequestLogger
        public void LogRequestData(string requstId, LoggerData data, string requestData)
        {
            _sb.AppendLine("rd");
        }

        public void LogRequestInfo(string requstId, LoggerData data, string msg)
        {
            _sb.AppendLine("ri");
        }

        public Exception LogRequestError(string requstId, LoggerData data, Exception e, string msg)
        {
            _sb.AppendLine("re");
            return e;
        }

        public void LogResponce(string requstId, LoggerData data, string responceData)
        {
            _sb.AppendLine("rs");
        }
        #endregion
    }
}
