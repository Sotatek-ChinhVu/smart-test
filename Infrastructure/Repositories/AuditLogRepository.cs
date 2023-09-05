using Domain.Models.AuditLog;
using Entity.Tenant;
using Helper.Common;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class AuditLogRepository : RepositoryBase, IAuditLogRepository
{
    public AuditLogRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public bool SaveAuditLog(int hpId, int userId, AuditTrailLogModel auditTrailLogModel)
    {
        AuditTrailLog auditTrailLog = new AuditTrailLog();
        auditTrailLog.HpId = hpId;
        auditTrailLog.PtId = auditTrailLogModel.PtId;
        auditTrailLog.SinDay = auditTrailLogModel.SinDate;
        auditTrailLog.UserId = userId;
        auditTrailLog.RaiinNo = auditTrailLogModel.RaiinNo;
        auditTrailLog.EventCd = auditTrailLogModel.EventCd;
        auditTrailLog.LogDate = CIUtil.GetJapanDateTimeNow();
        TrackingDataContext.AuditTrailLogs.Add(auditTrailLog);
        TrackingDataContext.SaveChanges();
        string hosoku = auditTrailLogModel.AuditTrailLogDetailModel.Hosoku;

        if (string.IsNullOrEmpty(hosoku) == false)
        {
            // 補足が必要な場合は、AUDIT_TRAIL_LOG_DETAILに保存
            AuditTrailLogDetail auditTrailLogDetailMode = new AuditTrailLogDetail();
            auditTrailLogDetailMode.LogId = auditTrailLogModel.LogId;
            auditTrailLogDetailMode.Hosoku = hosoku;
            TrackingDataContext.AuditTrailLogDetails.Add(auditTrailLogDetailMode);
        }

        return TrackingDataContext.SaveChanges() > 0;
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }

}
