using System;

namespace AuditTree.Core.AuditLogger
{
    public interface IAuditLogger
    {
        void Log(LoggerData data, string msg, string logData);
        void LogInfo(LoggerData data, string msg);
        Exception LogError(LoggerData data, Exception e, string msg = null);
    }


    public interface IRequestLogger
    {
        void LogRequestData(string requstId, LoggerData data,
            string requestData);

        void LogRequestInfo(string requstId, LoggerData data, string msg);

        Exception LogRequestError(string requstId, LoggerData data,
            Exception e, string msg);

        void LogResponce(string requstId, LoggerData data, string responceData);
    }
}
