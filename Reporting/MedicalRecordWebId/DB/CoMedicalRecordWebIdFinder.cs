using Domain.Constant;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Reporting.MedicalRecordWebId.Model;

namespace Reporting.MedicalRecordWebId.DB;

public class CoMedicalRecordWebIdFinder : RepositoryBase, ICoMedicalRecordWebIdFinder
{
    public CoMedicalRecordWebIdFinder(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }

    public CoHpInfModel GetHpInf(int hpId, int sinDate)
    {
        return new CoHpInfModel(NoTrackingDataContext.HpInfs
            .Where(item =>
                item.HpId == hpId &&
                item.StartDate <= sinDate)
            .OrderByDescending(p => p.StartDate)
            .FirstOrDefault() ?? new());
    }

    public CoPtInfModel GetPtInf(int hpId, long ptId)
    {
        return new CoPtInfModel(NoTrackingDataContext.PtInfs
            .FirstOrDefault(item =>
                item.HpId == hpId &&
                item.PtId == ptId &&
                item.IsDelete == DeleteStatus.None
            ) ?? new());
    }

    public CoPtJibkarModel GetPtJibkar(int hpId, long ptId)
    {
        string webId = NoTrackingDataContext.PtJibkars.FirstOrDefault(item => item.HpId == hpId && item.PtId == ptId)?.WebId ?? string.Empty;
        return new CoPtJibkarModel(NoTrackingDataContext.PtJibkars
            .FirstOrDefault(item =>
                item.HpId == hpId &&
                item.WebId == webId
            ) ?? new());
    }
}
