using Entity.Tenant;
using EventProcessor.Interfaces;
using EventProcessor.Model;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using PostgreDataContext;

namespace EventProcessor.Service;

public class EventProcessorService : IEventProcessorService
{
    private readonly TenantDataContext _tenantDataContext;

    public EventProcessorService(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
    }
    public bool DoEvent(List<ArgumentModel> listAuditTraiLogModels)
    {
        var executionStrategy = _tenantDataContext.Database.CreateExecutionStrategy();

        return executionStrategy.Execute(() =>
        {
            using var transaction = _tenantDataContext.Database.BeginTransaction();
            try
            {
                foreach (var item in listAuditTraiLogModels)
                {
                    AuditTrailLog auditTrailLog = new();
                    auditTrailLog.HpId = item.HpId;
                    auditTrailLog.PtId = item.PtId;
                    auditTrailLog.SinDay = item.SinDay;
                    auditTrailLog.UserId = item.UserId;
                    auditTrailLog.RaiinNo = item.RaiinNo;
                    auditTrailLog.EventCd = item.EventCd;
                    auditTrailLog.LogDate = DateTime.UtcNow;
                    _tenantDataContext.AuditTrailLogs.Add(auditTrailLog);
                    _tenantDataContext.SaveChanges();

                    if (!string.IsNullOrEmpty(item.Hosoku))
                    {
                        // 補足が必要な場合は、AUDIT_TRAIL_LOG_DETAILに保存
                        AuditTrailLogDetail auditTrailLogDetail = new();
                        auditTrailLogDetail.LogId = auditTrailLog.LogId;
                        auditTrailLogDetail.Hosoku = item.Hosoku;
                        _tenantDataContext.AuditTrailLogDetails.Add(auditTrailLogDetail);
                    }
                }
                _tenantDataContext.SaveChanges();
                return true;
            }
            catch
            {
                transaction.Rollback();

                return false;
            }
        });
    }
}
