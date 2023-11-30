using Amazon;
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
        AuditTrailLog auditTrailLog = new();
        auditTrailLog.HpId = hpId;
        auditTrailLog.PtId = auditTrailLogModel.PtId;
        auditTrailLog.SinDay = auditTrailLogModel.SinDate;
        auditTrailLog.UserId = userId;
        auditTrailLog.RaiinNo = auditTrailLogModel.RaiinNo;
        auditTrailLog.EventCd = auditTrailLogModel.EventCd;
        auditTrailLog.LogDate = CIUtil.GetJapanDateTimeNow();
        TrackingDataContext.AuditTrailLogs.Add(auditTrailLog);
        var saveAuditLog = TrackingDataContext.SaveChanges();
        string hosoku = auditTrailLogModel.AuditTrailLogDetailModel.Hosoku;

        if (!string.IsNullOrEmpty(hosoku))
        {
            // 補足が必要な場合は、AUDIT_TRAIL_LOG_DETAILに保存
            AuditTrailLogDetail auditTrailLogDetailMode = new AuditTrailLogDetail();
            auditTrailLogDetailMode.LogId = auditTrailLog.LogId;
            auditTrailLogDetailMode.Hosoku = hosoku;
            TrackingDataContext.AuditTrailLogDetails.Add(auditTrailLogDetailMode);
        }

        return saveAuditLog > 0 || TrackingDataContext.SaveChanges() > 0;
    }

    public void AddAuditTrailLog(int hpId, int userId, ArgumentModel arg)
    {
        var eventMsts = GetEventMstModel();

        if (eventMsts.Any(p => p.EventCd == arg.EventCd && p.AuditTrailing == 1))
        {
            var auditTrailLog = new AuditTrailLog();

            auditTrailLog.HpId = hpId;
            auditTrailLog.PtId = arg.PtId;
            auditTrailLog.SinDay = arg.SinDate;
            auditTrailLog.UserId = userId;
            auditTrailLog.RaiinNo = arg.RaiinNo;
            auditTrailLog.EventCd = arg.EventCd;
            auditTrailLog.LogDate = CIUtil.GetJapanDateTimeNow();

            TrackingDataContext.AuditTrailLogs.Add(auditTrailLog);
            TrackingDataContext.SaveChanges();

            if (!string.IsNullOrEmpty(arg.Hosoku))
            {
                var auditTrailLogDetail = new AuditTrailLogDetail();
                auditTrailLogDetail.LogId = auditTrailLog.LogId;
                auditTrailLogDetail.Hosoku = arg.Hosoku;
                TrackingDataContext.AuditTrailLogDetails.Add(auditTrailLogDetail);
                TrackingDataContext.SaveChanges();
            }
        }
    }

    public void AddListAuditTrailLog(int hpId, int userId, List<ArgumentModel> args)
    {
        var eventMsts = GetEventMstModel();

        foreach (var arg in args)
        {
            if (eventMsts.Any(p => p.EventCd == arg.EventCd && p.AuditTrailing == 1))
            {
                var auditTrailLog = new AuditTrailLog();

                auditTrailLog.HpId = hpId;
                auditTrailLog.PtId = arg.PtId;
                auditTrailLog.SinDay = arg.SinDate;
                auditTrailLog.UserId = userId;
                auditTrailLog.RaiinNo = arg.RaiinNo;
                auditTrailLog.EventCd = arg.EventCd;
                auditTrailLog.LogDate = CIUtil.GetJapanDateTimeNow();

                TrackingDataContext.AuditTrailLogs.Add(auditTrailLog);
                TrackingDataContext.SaveChanges();

                if (!string.IsNullOrEmpty(arg.Hosoku))
                {
                    var auditTrailLogDetail = new AuditTrailLogDetail();
                    auditTrailLogDetail.LogId = auditTrailLog.LogId;
                    auditTrailLogDetail.Hosoku = arg.Hosoku;
                    TrackingDataContext.AuditTrailLogDetails.Add(auditTrailLogDetail);
                    TrackingDataContext.SaveChanges();
                }
            }
        }
        
    }

    private List<EventMstModel> GetEventMstModel()
    {
        var eventMsts = NoTrackingDataContext.EventMsts
                                             .Where(p => p.AuditTrailing == 1)
                                             .Select(x => new EventMstModel(
                                                                            x.EventCd ?? string.Empty,
                                                                            x.EventName ?? string.Empty,
                                                                            x.AuditTrailing,
                                                                            x.CreateDate
                                             )).ToList();

        return eventMsts ?? new();
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }

}
