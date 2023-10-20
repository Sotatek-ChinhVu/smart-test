using Infrastructure.Common;

namespace Infrastructure.Logger
{
    public interface ILoggingHandler : IDisposable
    {
        Task WriteLogStartAsync(string message = "");

        Task WriteLogExceptionAsync(Exception exception, string message = "");

        Task WriteLogEndAsync(string message = "");

        Task WriteLogMessageAsync(string message);

        bool WriteAuditLog(string path, string requestInfo, string eventCd, long ptId, long raiinNo, int sinDay, string description, string logType);

        bool WriteAuditLog(List<AuditLogModel> auditLogList);
    }
}
