﻿using Entity.Logger;
using Helper.Common;
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
            AuditLog audit = new AuditLog()
            {
                Domain = _tenantProvider.GetDomain(),
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
                LogType = GetLogType(type)
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

        public bool WriteAuditLog(string requestInfo, string eventCd, long ptId, long raiinNo, int sinDay, string description, string logType)
        {
            AuditLog audit = new AuditLog()
            {
                Domain = _tenantProvider.GetDomain(),
                ClientIP = _tenantProvider.GetClientIp(),
                HpId = _tenantProvider.GetHpId(),
                DepartmentId = _tenantProvider.GetDepartmentId(),
                UserId = _tenantProvider.GetUserId(),
                TenantId = _tenantProvider.GetClinicID(),
                RequestInfo = requestInfo,
                LogDate = CIUtil.GetJapanDateTimeNow(),
                ThreadId = Thread.CurrentThread.ManagedThreadId.ToString(),
                EventCd = eventCd,
                PtId = ptId,
                RaiinNo = raiinNo,
                SinDay = sinDay,
                Desciption = description,
                LogType = logType
            };

            AuditLogs.Add(audit);

            return SaveChanges() > 0;
        }
    }
}
