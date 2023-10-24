using Entity.Logger;
using Helper.Common;
using Infrastructure.Common;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using PostgreDataContext;

namespace Infrastructure.Logger
{
    public class LoggingHandler : AdminDataContext, ILoggingHandler
    {
        private readonly ITenantProvider _tenantProvider;
        public LoggingHandler(DbContextOptions options, ITenantProvider tenantProvider) : base(options)
        {
            _tenantProvider = tenantProvider;
        }

        public async Task WriteLogStartAsync(string message = "")
        {
            AuditLogs.Add(await GenerateAuditLog(1, string.Empty, 0, 0, 0, message));
            SaveChanges();
        }

        public async Task WriteLogExceptionAsync(Exception exception, string message = "")
        {
            string info = string.Empty;
            if (!string.IsNullOrEmpty(message))
            {
                info = $"Info: {message} ";
            }
            string description = $"{info}Message: {exception.Message}{Environment.NewLine}StackTrace: {exception.StackTrace}";
            AuditLogs.Add(await GenerateAuditLog(3, string.Empty, 0, 0, 0, description));
            SaveChanges();
        }

        public async Task WriteLogEndAsync(string message = "")
        {
            AuditLogs.Add(await GenerateAuditLog(2, string.Empty, 0, 0, 0, message));
            SaveChanges();
        }

        public async Task WriteLogMessageAsync(string message)
        {
            AuditLogs.Add(await GenerateAuditLog(4, string.Empty, 0, 0, 0, message));
            SaveChanges();
        }

        public async Task<AuditLog> GenerateAuditLog(int type, string eventCd, long ptId, long raiinNo, int sinDay, string description)
        {
            var domain = _tenantProvider.GetDomain();
            if (domain.Contains("uat-tenant.smartkarte.org"))
            {
                domain = "UAT";
            }
            else if (domain.Contains("smartkarte-api.sotatek.works"))
            {
                domain = "SMARTKARTE";
            }
            else
            {
                domain = string.Empty;
            }

            AuditLog audit = new AuditLog()
            {
                Domain = domain,
                ClientIP = _tenantProvider.GetClientIp(),
                HpId = _tenantProvider.GetHpId(),
                DepartmentId = _tenantProvider.GetDepartmentId(),
                UserId = _tenantProvider.GetUserId(),
                TenantId = _tenantProvider.GetClinicID(),
                RequestInfo = await _tenantProvider.GetRequestInfoAsync(),
                LogDate = CIUtil.GetJapanDateTimeNow(),
                ThreadId = Thread.CurrentThread.ManagedThreadId.ToString(),
                EventCd = eventCd,
                PtId = ptId,
                RaiinNo = raiinNo,
                SinDay = sinDay,
                Desciption = description,
                LogType = GetLogType(type),
                LoginKey = _tenantProvider.GetLoginKeyFromHeader()
            };
            return audit;
        }

        private string GetLogType(int type)
        {
            switch (type)
            {
                case 1:
                    return "START";
                case 2:
                    return "END";
                case 3:
                    return "ERROR";
                case 4:
                    return "INFO";
                default:
                    return string.Empty;
            }
        }

        public bool WriteAuditLog(string path, string requestInfo, string eventCd, long ptId, long raiinNo, int sinDay, string description, string logType)
        {
            var domain = _tenantProvider.GetDomainFromHeader();
            if (domain.Contains("uat-tenant.smartkarte.org"))
            {
                domain = "UAT";
            }
            else if (domain.Contains("smartkarte-api.sotatek.works"))
            {
                domain = "SMARTKARTE";
            }
            else
            {
                domain = string.Empty;
            }

            AuditLog audit = new AuditLog()
            {
                Domain = domain,
                HpId = _tenantProvider.GetHpId(),
                DepartmentId = _tenantProvider.GetDepartmentId(),
                UserId = _tenantProvider.GetUserId(),
                TenantId = domain,
                Path = path,
                RequestInfo = requestInfo,
                LogDate = CIUtil.GetJapanDateTimeNow(),
                EventCd = eventCd,
                PtId = ptId,
                RaiinNo = raiinNo,
                SinDay = sinDay,
                Desciption = description,
                LogType = logType,
                LoginKey = _tenantProvider.GetLoginKeyFromHeader()
            };

            AuditLogs.Add(audit);

            return SaveChanges() > 0;
        }

        public bool WriteAuditLog(List<AuditLogModel> auditLogList)
        {
            string domain = _tenantProvider.GetDomainFromHeader().Replace(".org", "").Replace(".works", "");
            int hpId = _tenantProvider.GetHpId();
            int departmentId = _tenantProvider.GetDepartmentId();
            int userId = _tenantProvider.GetUserId();
            string tenantId = _tenantProvider.GetDomainFromHeader().Replace(".org", "").Replace(".works", "");

            if (domain.Contains("uat-tenant.smartkarte.org"))
            {
                domain = "UAT";
            }
            else if (domain.Contains("smartkarte-api.sotatek.works"))
            {
                domain = "SMARTKARTE";
            }
            else
            {
                domain = string.Empty;
            }

            foreach (var item in auditLogList)
            {
                AuditLog audit = new AuditLog()
                {
                    Domain = domain,
                    HpId = hpId,
                    DepartmentId = departmentId,
                    UserId = userId,
                    TenantId = domain,
                    Path = item.Path,
                    RequestInfo = item.RequestInfo,
                    LogDate = CIUtil.GetJapanDateTimeNow(),
                    EventCd = item.EventCd,
                    PtId = item.PtId,
                    RaiinNo = item.RaiinNo,
                    SinDay = item.SinDay,
                    Desciption = item.Description,
                    LogType = item.LogType,
                    LoginKey = _tenantProvider.GetLoginKeyFromHeader()
                };
                AuditLogs.Add(audit);
            }
            return SaveChanges() > 0;
        }
    }
}
