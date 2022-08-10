using Domain.Models.PtCmtInf;
using Domain.Models.PtOtcDrug;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class PtOtcDrugRepository : IPtOtcDrugRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public PtOtcDrugRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
    }

    public List<PtOtcDrugModel> GetList(long ptId)
    {
        var ptOtcDrugs = _tenantDataContext.PtOtcDrug.Where(x => x.PtId == ptId && x.IsDeleted == 0).Select(x => new PtOtcDrugModel(
                x.HpId,
                x.PtId,
                x.SeqNo,
                x.SortNo,
                x.SerialNum,
                x.TradeName,
                x.StartDate,
                x.EndDate,
                x.Cmt,
                x.IsDeleted
            ));
        return ptOtcDrugs.ToList();
    }
}
