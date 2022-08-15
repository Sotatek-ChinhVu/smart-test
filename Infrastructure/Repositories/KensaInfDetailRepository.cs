using Domain.Models.KensaInfDetail;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class KensaInfDetailRepository : IKensaInfDetailRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public KensaInfDetailRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
    }

    public List<KensaInfDetailModel> GetList(int hpId, long ptId, int sinDate)
    {
        var ptInfections = _tenantDataContext.KensaInfDetails.Where(x => x.HpId == hpId && x.PtId == ptId && x.IsDeleted == 0 && x.KensaItemCd.StartsWith("V") && x.IraiCd <= sinDate && !string.IsNullOrEmpty(x.ResultVal)).Select(x => new KensaInfDetailModel(
              x.HpId,
              x.PtId,
              x.IraiCd,
              x.SeqNo,
              x.IraiDate,
              x.RaiinNo,
              x.KensaItemCd ?? String.Empty,
              x.ResultVal ?? String.Empty,
              x.ResultType ?? String.Empty,
              x.AbnormalKbn ?? String.Empty,
              x.IsDeleted,
              x.CmtCd1 ?? String.Empty,
              x.CmtCd2 ?? String.Empty,
              x.UpdateDate
            ));
        return ptInfections.ToList();
    }
}
