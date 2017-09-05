using AuditTree.Core.AuditLogger;
using AuditTree.Core.RequestInfo;
using AuditTree.Core.SessionInfo;
using AuditTree.Core.UserInfo;

namespace AuditTree.Core.Audit
{
    public interface IAuditProvider
    {
        Audit GetAuditor(string className, string method, Audit parentAudit = null);
    }

    public class AuditProvider : IAuditProvider
    {
        private readonly IRequestInfoService _requestInfo;
        private readonly ISessionInfoService _sessionInfo;
        private readonly IUserInfoProvider _userInfo;
        private readonly IAuditLogger _auditLogger;
        private readonly IRequestLogger _requestLogger;

        public AuditProvider(IRequestInfoService requestInfo, ISessionInfoService sessionInfo, IUserInfoProvider userInfo, IAuditLogger auditLogger, IRequestLogger requestLogger)
        {
            _requestInfo = requestInfo;
            _sessionInfo = sessionInfo;
            _userInfo = userInfo;
            _auditLogger = auditLogger;
            _requestLogger = requestLogger;
        }


        public Audit GetAuditor(string className, string method, Audit parentAudit = null)
        {
            var layerNum = parentAudit?.LayerNum + 1 ?? 0;
            var layer = $"{className}.{method}({layerNum})";

            return new Audit(
                layer, layerNum,
                () => GetData(layer, parentAudit),
                _auditLogger, _requestLogger);
        }


        private LoggerData GetData(string layer, Audit parentAudit)
        {
            if (parentAudit == null)
            {
                return new LoggerData
                {
                    LayerId = layer, 
                    RequestId = _requestInfo.GetCurrentRequestId(),
                    SessionId = _sessionInfo.GetSessionId(),
                    UserId = _userInfo.GetUserId()
                };
            }

            var data = parentAudit.LoggerData.Value;
            return new LoggerData
            {
                LayerId = layer,
                RequestId = data.RequestId,
                ParentLayerId = data.LayerId,
                SessionId = data.SessionId,
                UserId = data.UserId
            };
        }
    }
}
