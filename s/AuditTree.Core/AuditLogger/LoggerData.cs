namespace AuditTree.Core.AuditLogger
{
    public class LoggerData
    {
        public string SessionId { get; set; }
        public string UserId { get; set; }
        public string RequestId { get; set; }

        public string LayerId { get; set; }
        public string ParentLayerId { get; set; }
    }
}
