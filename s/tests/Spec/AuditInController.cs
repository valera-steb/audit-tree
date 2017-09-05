using System.Text;
using AuditTree.Core.Audit;
using AuditTree.Core.RequestInfo;
using AuditTree.Core.SessionInfo;
using AuditTree.Core.UserInfo;
using NSubstitute;
using Spec.Stubs;
using Xbehave;
using FluentAssertions;

namespace Spec
{
    public class AuditInController
    {
        [Scenario]
        public void X(object di, object scope, object audit)
        {
            "Given DI with registrations".x(
                () => di = "build di");

            "Build lifetime scope".x(
                () => scope = "resolved scope"
            );

            "Wroute some audit in controller".x(
                () => audit = "some audit data");

            "Expected message".x(
                () => { });
        }


        [Scenario]
        public void SimpleAudit(IAuditProvider auditProvider,
            Audit audit, Audit subAudit, StringBuilder sb)
        {
            "mock up providers".x(() =>
            {
                var requestInfo = Substitute.For<IRequestInfoService>();
                requestInfo.GetCurrentRequestId().Returns("request_id");

                var sessionInfo = Substitute.For<ISessionInfoService>();
                sessionInfo.GetSessionId().Returns("session_id");

                var userInfo = Substitute.For<IUserInfoProvider>();
                userInfo.GetUserId().Returns("user_id");

                sb = new StringBuilder().AppendLine();

                var auditLogger = new StringAuditLogger(sb);

                auditProvider = new AuditProvider(
                    requestInfo, sessionInfo, userInfo, auditLogger, auditLogger
                );
            });

            "get auditor in controller".x(() =>
            {
                audit = auditProvider.GetAuditor("controller", "method");
                audit.LogInfo("info from controller");

                sb.ToString().Should().Be(@"
controller.method(0) - request_id.session_id.user_id - info from controller
");
            });

            "get sub scope auditor for service".x(() =>
            {
                subAudit = auditProvider.GetAuditor("service1", "method1", audit);

                subAudit.LogInfo("info in service", "some data 1");

                sb.ToString().Should().Be(@"
controller.method(0) - request_id.session_id.user_id - info from controller
controller.method(0)_service1.method1(1) - request_id.session_id.user_id - info in service
");
            });

            "audit request".x(() =>
            {
                var requestAudit = subAudit.GetRequestAuditor();

                requestAudit.LogRequest("request data");
                requestAudit.LogResponce("responce data");

                sb.ToString().Should().StartWith(@"
controller.method(0) - request_id.session_id.user_id - info from controller
controller.method(0)_service1.method1(1) - request_id.session_id.user_id - info in service
controller.method(0)_service1.method1(1) - request_id.session_id.user_id - start request");
                sb.ToString().Should().EndWith(@"
rd
rs
");
            });
        }
    }
}
