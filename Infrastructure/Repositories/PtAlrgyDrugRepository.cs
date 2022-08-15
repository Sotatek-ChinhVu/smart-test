using Domain.Models.PtAlrgyDrug;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories;

public class PtAlrgyDrugRepository : IPtAlrgyDrugRepository
{
    private readonly TenantDataContext _tenantDataContext;

    public PtAlrgyDrugRepository(ITenantProvider tenantProvider)
    {
        _tenantDataContext = tenantProvider.GetTrackingTenantDataContext();
    }

    public List<PtAlrgyDrugModel> GetList(long ptId)
    {
        var ptAlrgyDrugs = _tenantDataContext.PtAlrgyDrugs.Where(x => x.PtId == ptId && x.IsDeleted == 0).Select(x => new PtAlrgyDrugModel(
               x.HpId,
               x.PtId,
               x.SeqNo,
               x.SortNo,
               x.ItemCd ?? String.Empty,
               x.DrugName ?? String.Empty,
               x.StartDate,
               x.EndDate,
               x.Cmt ?? String.Empty,
               x.IsDeleted
            ));
        return ptAlrgyDrugs.ToList();
    }
}
