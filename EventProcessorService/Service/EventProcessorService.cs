using Entity.Tenant;
using EventProcessor.Interfaces;
using EventProcessor.Model;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using PostgreDataContext;

namespace EventProcessor.Service;

public class EventProcessorService : IEventProcessorService
{
    private readonly TenantNoTrackingDataContext _tenantNoTrackingDataContext;
    private readonly TenantDataContext _tenantDataContext;

    public EventProcessorService(ITenantProvider tenantProvider)
    {
        _tenantNoTrackingDataContext = tenantProvider.GetNoTrackingDataContext();
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

    public bool BundleExecute(BundleArgumentModel bundleArg)
    {
        throw new NotImplementedException();
    }

    public bool CheckOdrItem(long ptId, int sinDate, long raiinNo, string biko)
    {
        throw new NotImplementedException();
    }

    public CommonDataModel GetCommonData(ArgumentModel auditTraiLogModel)
    {
        throw new NotImplementedException();
    }

    public Renkei040DataModel GetRenkei040Data(long ptId, int sinDate, long raiinNo)
    {
        throw new NotImplementedException();
    }

    public Renkei050DataModel GetRenkei050Data(long ptId, int sinDate, long raiinNo)
    {
        throw new NotImplementedException();
    }

    public Renkei060DataModel GetRenkei060Data(long ptId)
    {
        throw new NotImplementedException();
    }

    public Renkei070DataModel GetRenkei070Data(long ptId)
    {
        throw new NotImplementedException();
    }

    public Renkei080DataModel GetRenkei080Data(long ptId, int sinDate, long raiinNo)
    {
        throw new NotImplementedException();
    }

    public Renkei100DataModel GetRenkei100Data(long ptId, int sinDate, long raiinNo)
    {
        throw new NotImplementedException();
    }

    public List<Renkei130DataModel> GetRenkei130Data(ArgumentModel arg)
    {
        throw new NotImplementedException();
    }

    public List<Renkei260OdrInfModel> GetRenkei260Data(long ptId, int sinDate)
    {
        throw new NotImplementedException();
    }

    public List<Renkei270KarteInfModel> GetRenkei270Data(long ptId, int sinDate)
    {
        throw new NotImplementedException();
    }

    public Renkei280DataModel GetRenkei280Data(long ptId, int sinDate, long raiinNo)
    {
        throw new NotImplementedException();
    }

    public Renkei330DataModel GetRenkei330Data(long ptId, int sinDate, long raiinNo, string eventCd, string hosoku)
    {
        throw new NotImplementedException();
    }

    public Renkei350DataModel GetRenkei350Data(long ptId, int sinDate)
    {
        throw new NotImplementedException();
    }

    public Renkei360DataModel GetRenkei360Data(long ptId, int sinDate, long raiinNo)
    {
        throw new NotImplementedException();
    }

    public bool RunCommonProgram(RenkeiModel renkeiModel, CommonDataModel common)
    {
        throw new NotImplementedException();
    }
}
